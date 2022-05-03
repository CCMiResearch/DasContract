import { toBase64 } from 'js-base64';

export function saveFile(download, href) {
    const a = document.createElement('a');
    a.download = download;
    a.href = href;
    a.click();
    a.remove();
}

export function saveContract(fileName, fileContent) {
	var FileSaver = require('file-saver');
    var blob = new Blob([fileContent], { type: "text/plain;charset=utf-8" });
    FileSaver.saveAs(blob, fileName);
}

export function downloadSvg(fileName, svgString) {
    saveFile(
        fileName,
        `data:image/svg+xml;base64,${toBase64(svgString)}`
    );
}

export function downloadSvgAsPng(fileName, svgString, width, height) {
	debugger;
	exportImage(fileName, downloadImage, svgString, width, height);
}

const exportImage = (diagramName, exporter, svgString, width, height) => {
	const canvas = document.createElement('canvas');
	canvas.width = width;
	canvas.height = height;

	const context = canvas.getContext('2d');
	context.fillStyle = 'white';
	context.fillRect(0, 0, canvas.width, canvas.height);
	const image = new Image();
	image.onload = exporter(context, image, diagramName);
	image.src = `data:image/svg+xml;base64,${toBase64(svgString)}`;
};

const downloadImage = (context, image, fileName) => {
	return () => {
		const { canvas } = context;
		context.drawImage(image, 0, 0, canvas.width, canvas.height);
		saveFile(
			fileName,
			canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream')
		);
	};
};