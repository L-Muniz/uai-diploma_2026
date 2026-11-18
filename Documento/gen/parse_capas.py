import re, os

ROOT = "C:/Users/Lucía/source/repos/L-Muniz/uai-diploma_2026/.claude/worktrees/pdf-con-imagenes-32ab07/Proyecto"
OUT = "C:/Users/Lucía/source/repos/L-Muniz/uai-diploma_2026/.claude/worktrees/pdf-con-imagenes-32ab07/Documento/Diagramas_Entrega3"
CAPAS = ["BE", "DAL", "BLL", "SERVICE"]

CLASS_RE = re.compile(r"^\s*public\s+((?:abstract|static|sealed|partial)\s+)*(class|interface)\s+(\w+)(<[^>]+>)?\s*(:\s*([^{]+?))?\s*(\{.*)?$")
PROP_RE = re.compile(r"^\s*(public|private|protected|internal)\s+((?:static|readonly|const)\s+)*([\w<>\[\],\.\? ]+?)\s+(\w+)\s*(\{|=|;)")
METH_RE = re.compile(r"^\s*(public|protected|private|internal)\s+((?:static|virtual|override|abstract|async|new)\s+)*([\w<>\[\],\.\?\(\) ]+?)\s+(\w+)\s*\(([^)]*)\)\s*(\{|=>|;)?\s*$")
CTOR_RE = re.compile(r"^\s*(public|protected|private|internal)\s+(\w+)\s*\(([^)]*)\)\s*(\{|:)?\s*$")


def clean(t):
    return re.sub(r"\s+", " ", t.strip())


def parse_file(path):
    classes = []
    lines = open(path, encoding="utf-8-sig", errors="ignore").read().split("\n")
    depth = 0
    cur = None
    for line in lines:
        code = line.split("//")[0]
        d_before = depth
        opens = code.count("{")
        closes = code.count("}")
        if cur is None:
            m = CLASS_RE.match(code)
            if m:
                cur = {"name": m.group(3), "kind": m.group(2), "base": (m.group(6) or "").strip(),
                       "attrs": [], "meths": [], "static": "static" in (m.group(1) or ""),
                       "abstract": "abstract" in (m.group(1) or ""), "body": None}
                classes.append(cur)
        else:
            if cur["body"] is not None and d_before == cur["body"]:
                s = code.strip()
                mm = METH_RE.match(code)
                cm = CTOR_RE.match(code)
                pm = PROP_RE.match(code)
                if mm and "=" not in code.split("(")[0] and not s.startswith(("return", "throw")):
                    vis, _, ret, name, params, _ = mm.groups()
                    if vis in ("public", "protected"):
                        cur["meths"].append((vis, clean(ret), name, clean(params)))
                elif cm and cm.group(2) == cur["name"]:
                    if cm.group(1) == "public":
                        cur["meths"].append(("public", "", cur["name"], clean(cm.group(3))))
                elif pm and "(" not in code.split("=")[0] and not s.startswith(("return", "throw", "var")):
                    vis, _, typ, name, _ = pm.groups()
                    cur["attrs"].append((vis, clean(typ), name))
        depth = d_before + opens - closes
        if cur is not None:
            if cur["body"] is None and opens > 0:
                cur["body"] = d_before + 1
            if cur["body"] is not None and depth < cur["body"]:
                cur = None
    return classes


def collect(capa):
    base = os.path.join(ROOT, capa)
    res = []
    for dp, dn, fn in os.walk(base):
        parts = dp.replace("\\", "/").split("/")
        if "Properties" in parts or "obj" in parts or "bin" in parts:
            continue
        for f in fn:
            if f.endswith(".cs") and "AssemblyInfo" not in f and ".Designer" not in f:
                res += parse_file(os.path.join(dp, f))
    return res


def emit(capa, classes):
    names = {c["name"] for c in classes}
    out = [f"@startuml CAPA_{capa}", f"title Capa {capa} - Diagrama de clases",
           "skinparam classAttributeIconSize 0", "skinparam linetype ortho", ""]
    for c in classes:
        stereo = ""
        if c["kind"] == "interface":
            stereo = " <<interface>>"
        elif c["static"]:
            stereo = " <<static>>"
        abst = "abstract " if c["abstract"] else ""
        out.append(f"{abst}class {c['name']}{stereo} {{")
        for vis, typ, name in c["attrs"]:
            sym = {"public": "+", "private": "-", "protected": "#"}.get(vis, "~")
            out.append(f"  {sym} {name} : {typ}")
        if c["attrs"] and c["meths"]:
            out.append("  --")
        for vis, ret, name, params in c["meths"]:
            sym = {"public": "+", "protected": "#"}.get(vis, "~")
            if ret:
                out.append(f"  {sym} {name}({params}) : {ret}")
            else:
                out.append(f"  {sym} {name}({params})")
        out.append("}")
        out.append("")
    rels = []
    for c in classes:
        if c["base"]:
            for b in [x.strip() for x in c["base"].split(",")]:
                bn = re.sub(r"<.*>", "", b).strip()
                if bn in names and bn != c["name"]:
                    rels.append(f"{c['name']} --|> {bn}")
        for vis, typ, name in c["attrs"]:
            t = re.sub(r"<.*>", "", typ).replace("[]", "").replace("?", "").strip()
            if t in names and t != c["name"]:
                rels.append(f"{c['name']} --> {t}")
    seen = []
    for r in rels:
        if r not in seen:
            seen.append(r)
    out.extend(seen)
    out.append("@enduml")
    path = os.path.join(OUT, f"CAPA_{capa}_clases.puml")
    open(path, "w", encoding="utf-8").write("\n".join(out) + "\n")
    return path, len(classes), len(seen)


if __name__ == "__main__":
    os.makedirs(OUT, exist_ok=True)
    for capa in CAPAS:
        cls = collect(capa)
        p, n, r = emit(capa, cls)
        print(capa, "clases:", n, "relaciones:", r)
