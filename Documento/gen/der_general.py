import re
OUT = "C:/Users/Lucía/source/repos/L-Muniz/uai-diploma_2026/.claude/worktrees/pdf-con-imagenes-32ab07/Documento/Diagramas_Entrega3"
pks, fks, cols = set(), [], {}
order = []
for raw in open("schema.txt", encoding="utf-16" if open("schema.txt","rb").read(2)==b"\xff\xfe" else "utf-8-sig"):
    raw = raw.strip()
    if not raw: continue
    parts = raw.split("|")
    if parts[0] == "PK":
        pks.add((parts[1], parts[2]))
    elif parts[0] == "FK":
        fks.append(tuple(parts[1:5]) + (parts[5],))
    elif parts[0] == "COL":
        t = parts[1]
        if t not in cols:
            cols[t] = []; order.append(t)
        cols[t].append((parts[2], parts[3], parts[4]))
fk_pairs = {(a, b) for a, b, c, d, n in fks}
L = ["@startuml DER_GENERAL", "title Modelo entidad-relación general (ProyectoC)", "hide circle", "skinparam linetype ortho", ""]
for t in order:
    L.append(f'entity "{t}" as {t} {{')
    for name, typ, ln in cols[t]:
        mark = ""
        is_pk = (t, name) in pks
        is_fk = (t, name) in fk_pairs
        star = "*" if is_pk else ""
        tags = []
        if is_pk: tags.append("PK")
        if is_fk: tags.append("FK")
        tag = f" <<{', '.join(tags)}>>" if tags else ""
        L.append(f"  {star}{name} : {typ}{tag}")
    L.append("}")
    L.append("")
for child, col, parent, pcol, nullable in fks:
    card = "|o" if nullable.strip() == "True" else "||"
    L.append(f"{parent} ||--o{{ {child}")
L.append("@enduml")
open(f"{OUT}/DER_GENERAL.puml", "w", encoding="utf-8").write("\n".join(L) + "\n")
print("tablas", len(order), "fks", len(fks))
