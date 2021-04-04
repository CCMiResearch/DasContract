import Split from 'split-grid';

export function createSplit(gutterSelector) {
    console.log("Split has been called")
    Split({
        columnGutters: [{
            track: 1,
            element: document.querySelector(gutterSelector)
        }]
    })
}