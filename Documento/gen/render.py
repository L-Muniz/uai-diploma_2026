"""Renderizadores simples (matplotlib) para los diagramas de la entrega.

- sequence(): diagrama de secuencia con actor, interfaz (límite) y clases internas.
- classes(): diagrama de clases con estereotipo, atributos y métodos con firma y retorno.
- er(): diagrama entidad-relación con PK/FK.
"""
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.patches import Ellipse, FancyBboxPatch, Polygon, Rectangle

FONT = "DejaVu Sans"
BOX_FILL = "#FFF8EC"
BOX_EDGE = "#C9A36B"
LIFE_FILL = "#CFEFF2"
LIFE_EDGE = "#4A9AA5"


def _txt(ax, x, y, s, size=7, ha="center", va="center", weight="normal", color="black", style="normal"):
    ax.text(x, y, s, fontsize=size, ha=ha, va=va, family=FONT, weight=weight, color=color, style=style)


# ---------------------------------------------------------------- SECUENCIA
def sequence(path, title, lifelines, events, step=0.42):
    """
    lifelines: lista de (id, rotulo, tipo) con tipo en {'actor','limite','entidad'}
    events: lista de tuplas:
        ('msg', origen, destino, texto, es_retorno)
        ('self', id, texto)
        ('frame', etiqueta)   -> abre un marco (alt / opt)
        ('end',)              -> cierra el último marco
    """
    n = len(lifelines)
    xs = {ll[0]: 1.0 + i * 2.1 for i, ll in enumerate(lifelines)}
    # calcular alto según eventos
    y = 0.0
    rows = []
    frames = []
    open_frames = []
    for ev in events:
        if ev[0] == "frame":
            y += 0.35
            open_frames.append((y, ev[1]))
            y += 0.25
            rows.append(("frame_open", y))
        elif ev[0] == "end":
            y += 0.3
            start, label = open_frames.pop()
            frames.append((start - 0.25, y, label))
            rows.append(("frame_close", y))
        else:
            y += step
            rows.append((ev, y))
    total_h = y + 1.6
    fig_w = max(10, 2.1 * n + 2.2)
    fig, ax = plt.subplots(figsize=(fig_w * 0.9, total_h * 0.55 + 1.2), dpi=170)
    ax.set_xlim(0, 1.0 + (n - 1) * 2.1 + 1.2)
    ax.set_ylim(-(total_h + 0.6), 1.2)
    ax.axis("off")

    # marco exterior y título
    frame_rect = Rectangle((0.05, -(total_h + 0.5)), 1.0 + (n - 1) * 2.1 + 1.1, total_h + 1.45,
                           fill=False, edgecolor="#777777", linewidth=0.6)
    ax.add_patch(frame_rect)
    _txt(ax, 0.2, 0.9, f"sd {title}", size=7.5, ha="left", weight="bold")

    # cabeceras y líneas de vida
    for lid, label, kind in lifelines:
        x = xs[lid]
        if kind == "actor":
            _draw_actor(ax, x, 0.55, label)
            top_y = -0.1
        elif kind == "limite":
            # símbolo de interfaz (boundary de UML): círculo con barra vertical y línea de conexión
            ax.add_patch(plt.Circle((x, 0.62), 0.14, fill=False, lw=0.8, ec=LIFE_EDGE))
            ax.plot([x - 0.5, x - 0.5], [0.44, 0.8], color=LIFE_EDGE, lw=0.8)
            ax.plot([x - 0.5, x - 0.14], [0.62, 0.62], color=LIFE_EDGE, lw=0.8)
            _txt(ax, x, 0.2, label, size=6.6, weight="bold")
            top_y = 0.05
        elif kind == "bd":
            # símbolo de base de datos (cilindro)
            ax.add_patch(Rectangle((x - 0.55, 0.22), 1.1, 0.4, fc=LIFE_FILL, ec=LIFE_EDGE, lw=0.7))
            ax.add_patch(Ellipse((x, 0.62), 1.1, 0.2, fc=LIFE_FILL, ec=LIFE_EDGE, lw=0.7))
            ax.add_patch(Ellipse((x, 0.22), 1.1, 0.2, fc=LIFE_FILL, ec=LIFE_EDGE, lw=0.7))
            _txt(ax, x, 0.42, label, size=6.6, weight="bold")
            top_y = 0.1
        else:
            box = FancyBboxPatch((x - 0.85, 0.2), 1.7, 0.42, boxstyle="round,pad=0.02",
                                 fc=LIFE_FILL, ec=LIFE_EDGE, lw=0.7)
            ax.add_patch(box)
            _txt(ax, x, 0.41, label, size=6.6, weight="bold")
            top_y = 0.2
        ax.plot([x, x], [top_y - 0.05, -(total_h + 0.2)], linestyle=(0, (4, 3)), color="#888888", lw=0.6)

    # marcos
    for top, bottom, label in frames:
        x0 = 0.25
        x1 = 1.0 + (n - 1) * 2.1 + 0.6
        ax.add_patch(Rectangle((x0, -bottom), x1 - x0, bottom - top, fill=False,
                               edgecolor="#B07A3A", lw=0.7))
        tag_w = 0.12 * len(label) + 0.5
        ax.add_patch(Polygon([[x0, -top], [x0 + tag_w, -top], [x0 + tag_w, -top - 0.22],
                              [x0 + tag_w - 0.12, -top - 0.36], [x0, -top - 0.36]],
                             closed=True, fc="#F5DDB6", ec="#B07A3A", lw=0.6))
        _txt(ax, x0 + 0.1, -top - 0.14, label, size=6.4, ha="left", weight="bold")

    # mensajes
    for kind, yy in rows:
        if kind in ("frame_open", "frame_close"):
            continue
        ev = kind
        if ev[0] == "msg":
            _, src, dst, text, ret = ev
            x1, x2 = xs[src], xs[dst]
            yy_ = -yy
            if ret:
                ax.annotate("", xy=(x2, yy_), xytext=(x1, yy_),
                            arrowprops=dict(arrowstyle="->", lw=0.6, linestyle=(0, (3, 2)), color="#333333"))
            else:
                ax.annotate("", xy=(x2, yy_), xytext=(x1, yy_),
                            arrowprops=dict(arrowstyle="-|>", lw=0.7, color="#222222"))
            _txt(ax, (x1 + x2) / 2, yy_ + 0.13, text, size=6.2)
        elif ev[0] == "self":
            _, lid, text = ev
            x = xs[lid]
            yy_ = -yy
            ax.annotate("", xy=(x + 0.5, yy_ - 0.14), xytext=(x, yy_),
                        arrowprops=dict(arrowstyle="-|>", lw=0.7, color="#222222",
                                        connectionstyle="angle,angleA=0,angleB=90,rad=2"))
            _txt(ax, x + 0.55, yy_ + 0.1, text, size=6.2, ha="left")
    fig.savefig(path, bbox_inches="tight", facecolor="white")
    plt.close(fig)


def _draw_actor(ax, x, y, label):
    ax.add_patch(plt.Circle((x, y + 0.2), 0.08, fill=False, lw=0.7, ec="#222222"))
    ax.plot([x, x], [y + 0.12, y - 0.2], color="#222222", lw=0.7)
    ax.plot([x - 0.15, x + 0.15], [y - 0.02, y - 0.02], color="#222222", lw=0.7)
    ax.plot([x, x - 0.13], [y - 0.2, y - 0.38], color="#222222", lw=0.7)
    ax.plot([x, x + 0.13], [y - 0.2, y - 0.38], color="#222222", lw=0.7)
    _txt(ax, x, y - 0.5, label, size=6.6, weight="bold")


# ---------------------------------------------------------------- CLASES
def classes(path, title, boxes, relations, width=16, height=10):
    """
    boxes: dict nombre -> {'x','y','estereotipo','atributos':[...], 'metodos':[...]} (x,y = esquina superior izquierda)
    relations: lista de (origen, destino, tipo) con tipo en {'dep','asoc','herencia'}
    """
    fig, ax = plt.subplots(figsize=(width, height), dpi=170)
    ax.set_xlim(0, width * 10)
    ax.set_ylim(-height * 10, 0)
    ax.axis("off")
    ax.add_patch(Rectangle((2, -height * 10 + 2), width * 10 - 4, height * 10 - 4, fill=False, ec="#777777", lw=0.6))
    _txt(ax, 6, -8, f"class {title}", size=8, ha="left", weight="bold")

    geo = {}
    for name, b in boxes.items():
        w = b.get("w", 34)
        lines_attr = b.get("atributos", [])
        lines_met = b.get("metodos", [])
        h = 9 + 4.2 * (1 + (1 if b.get("estereotipo") else 0)) + 4.2 * len(lines_attr) + 4.2 * len(lines_met) + 8
        x, y = b["x"], -b["y"]
        ax.add_patch(Rectangle((x, y - h), w, h, fc=BOX_FILL, ec=BOX_EDGE, lw=0.7))
        cy = y - 4
        if b.get("estereotipo"):
            _txt(ax, x + w / 2, cy - 3, f"«{b['estereotipo']}»", size=5.6, color="#555555")
            cy -= 6
        _txt(ax, x + w / 2, cy - 3.5, name, size=7, weight="bold")
        cy -= 7
        ax.plot([x, x + w], [cy, cy], color=BOX_EDGE, lw=0.5)
        cy -= 2.5
        for a in lines_attr:
            _txt(ax, x + 2.5, cy - 2, a, size=5.6, ha="left")
            cy -= 4.2
        if lines_attr and lines_met:
            ax.plot([x, x + w], [cy - 1, cy - 1], color=BOX_EDGE, lw=0.5)
            cy -= 3
        for m in lines_met:
            _txt(ax, x + 2.5, cy - 2, m, size=5.6, ha="left")
            cy -= 4.2
        geo[name] = (x, y, w, h)

    for orig, dest, tipo in relations:
        ox, oy, ow, oh = geo[orig]
        dx, dy, dw, dh = geo[dest]
        p1 = (ox + ow / 2, oy - oh / 2)
        p2 = (dx + dw / 2, dy - dh / 2)
        style = dict(lw=0.6, color="#333333")
        if tipo == "dep":
            ax.annotate("", xy=p2, xytext=p1, arrowprops=dict(arrowstyle="-|>", linestyle=(0, (3, 2)), **style))
        elif tipo == "herencia":
            ax.annotate("", xy=p2, xytext=p1, arrowprops=dict(arrowstyle="-|>", linestyle="-",
                        fc="white", **style))
        else:
            ax.annotate("", xy=p2, xytext=p1, arrowprops=dict(arrowstyle="-", **style))
    fig.savefig(path, bbox_inches="tight", facecolor="white")
    plt.close(fig)


# ---------------------------------------------------------------- ENTIDAD-RELACIÓN
def er(path, title, tables, relations, width=16, height=9):
    """
    tables: dict nombre -> {'x','y','columnas':[(nombre, tipo, marca)]} marca en {'PK','FK',''}
    relations: lista de (tabla_a, tabla_b, cardinalidad_a, cardinalidad_b, etiqueta)
    """
    fig, ax = plt.subplots(figsize=(width, height), dpi=170)
    ax.set_xlim(0, width * 10)
    ax.set_ylim(-height * 10, 0)
    ax.axis("off")
    ax.add_patch(Rectangle((2, -height * 10 + 2), width * 10 - 4, height * 10 - 4, fill=False, ec="#777777", lw=0.6))
    _txt(ax, 6, -8, f"DER {title}", size=8, ha="left", weight="bold")
    geo = {}
    for name, t in tables.items():
        w = t.get("w", 32)
        cols = t["columnas"]
        h = 9 + 4.4 * len(cols) + 4
        x, y = t["x"], -t["y"]
        ax.add_patch(Rectangle((x, y - h), w, h, fc="#EEF6FB", ec="#4A7FA5", lw=0.7))
        ax.add_patch(Rectangle((x, y - 8), w, 8, fc="#4A7FA5", ec="#4A7FA5", lw=0.7))
        _txt(ax, x + w / 2, y - 4, name, size=6.8, weight="bold", color="white")
        cy = y - 11
        for cname, ctipo, marca in cols:
            label = f"{marca:<3} {cname}" if marca else f"    {cname}"
            _txt(ax, x + 2.5, cy - 2, label, size=5.6, ha="left", weight="bold" if marca == "PK" else "normal")
            _txt(ax, x + w - 2.5, cy - 2, ctipo, size=5.2, ha="right", color="#555555")
            cy -= 4.4
        geo[name] = (x, y, w, h)
    for a, b, ca, cb, etiqueta in relations:
        ax_, ay, aw, ah = geo[a]
        bx, by, bw, bh = geo[b]
        p1 = (ax_ + aw / 2, ay - ah / 2)
        p2 = (bx + bw / 2, by - bh / 2)
        ax.plot([p1[0], p2[0]], [p1[1], p2[1]], color="#4A7FA5", lw=0.7)
        mx, my = (p1[0] + p2[0]) / 2, (p1[1] + p2[1]) / 2
        _txt(ax, mx, my + 2, f"{ca} — {cb}", size=5.8, color="#4A7FA5")
        if etiqueta:
            _txt(ax, mx, my - 2.6, etiqueta, size=5.2, color="#777777", style="italic")
    fig.savefig(path, bbox_inches="tight", facecolor="white")
    plt.close(fig)
