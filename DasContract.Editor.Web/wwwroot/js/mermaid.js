import mermaid from 'mermaid/dist/mermaid.min.js'

export function initialize() {
    mermaid.mermaidAPI.initialize({ startOnLoad: true, flowchart: {useMaxWidth:true} });
}

export function renderMermaidDiagram(elementId, input) {
    var canvas = document.getElementById(elementId);
    mermaid.mermaidAPI.render('graph', input, function (svgCode) {
        canvas.innerHTML = svgCode;
    })
}