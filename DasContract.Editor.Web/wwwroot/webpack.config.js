const path = require("path");

module.exports = {
    entry: {
        splitter: './js/splitter.js',
        modeller: './js/modeller.js',
        fileSaver: './js/fileSaver.js',
        modellerCustomRules: './js/modellerCustomRules.js',
        dmnModeller: './js/dmnModeller.js',
        keyCapture: './js/keyCapture.js',
        select2: './js/select2.js',
        mermaid: './js/mermaid.js',
        tooltips: './js/tooltips.js'
    },
    module: {
        rules: [ 
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            }
        ]
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: "[name]_bundle.js",
        library: "[name]Lib"
    },
    watch: true
};