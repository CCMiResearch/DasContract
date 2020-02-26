const path = require('path');
const webpack = require('webpack');

module.exports = [
    //FRONT-END
    {
        //Entry points
        entry: {
            Global: "./wwwroot/_dist/Scripts/Global.js"
        },

        //Output
        output: {
            filename: "[name].js",
            path: path.resolve(__dirname, "./wwwroot/_dist/_pack")
        },

        //Mode
        mode: "development",

        //Watcher
        watch: false,
        watchOptions: watchOptions = {
            ignored: /node_modules/
        },

        //plugins: [
        //    new webpack.ProvidePlugin({
        //        $: "jquery",
        //        jQuery: "jquery",
        //        Popper: 'popper.js/dist/umd/popper'
        //    })
        //],

        module: {
            rules: [

                //CSS
                {
                    test: /\.css$/,
                    use: ['style-loader', 'css-loader']
                },

                //SCSS
                {
                    test: /\.(scss)$/,
                    use: [{
                        loader: "style-loader"
                    }, {
                        loader: "css-loader"
                    }, {
                        loader: "postcss-loader",
                        options: {
                            plugins: function ()
                            {
                                return [
                                    require("precss"),
                                    require("autoprefixer")
                                ];
                            }
                        }
                    }, {
                        loader: 'sass-loader'
                    }]
                }
            ]
        }
    }
];