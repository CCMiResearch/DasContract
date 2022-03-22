import mermaid from 'mermaid/dist/mermaid.min.js';
import { downloadSvg, downloadSvgAsPng } from '../js/fileSaver';

const getSvgString = () => {
	const svg = $('#graph')[0];
	debugger;
	const svgString = svg.outerHTML
		.replaceAll('<br>', '<br/>')
		.replaceAll(/<img([^>]*)>/g, (m, g) => `<img ${g} />`);
	return svgString;
};


export const downloadSVG = (contractName) => {
	downloadSvg(
		`data-model-${contractName}.svg`,
		getSvgString()
	);
}

export const downloadPNG = (contractName) => {
	const svgElement = $('#graph')[0];
	const box = svgElement.getBoundingClientRect();
	downloadSvgAsPng(
		`data-model-${contractName}.png`,
		getSvgString(),
		box.width,
		box.height
	);
}

export function renderMermaidDiagram(elementId, input) {
	var canvas = document.getElementById(elementId);
	try {
		mermaid.mermaidAPI.render('graph', input, function (svgCode) {
			canvas.innerHTML = svgCode;
			$('#graph').removeAttr('height').removeAttr('width')
				.css({ maxWidth: "" });
		})
	} catch (e) {
		let dgraph = $('#graph')[0];
		dgraph.remove();
    }
}