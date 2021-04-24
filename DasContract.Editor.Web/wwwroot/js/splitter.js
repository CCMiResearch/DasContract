import Split from 'split-grid';

export function createSplit(gutterSelector) {
    Split({
        columnGutters: [{
            track: 1,
            element: document.querySelector(gutterSelector)
        }]
    })
}