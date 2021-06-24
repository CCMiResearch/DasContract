import mermaid from 'mermaid/dist/mermaid.min.js'

export function renderMermaidDiagram(elementId, input) {
    var canvas = document.getElementById(elementId);
    console.log(input);
    mermaid.mermaidAPI.render('graph', input, function (svgCode) {
        canvas.innerHTML = svgCode;
    })
}