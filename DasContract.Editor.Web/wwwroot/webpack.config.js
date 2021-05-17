const path = require("path");

module.exports = {
    entry: {
        splitter: './js/splitter.js',
        modeller: './js/modeller.js',
        fileSaver: './js/fileSaver.js'
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