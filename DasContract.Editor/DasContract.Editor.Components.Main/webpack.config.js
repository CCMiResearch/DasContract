const path = require('path');
const webpack = require('webpack');

module.exports = [
    //FRONT-END
    {
        //Entry points
        entry: {
            Global: "./wwwroot/_dist/wwwroot/Scripts/Global.js"
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

        plugins: [
            new webpack.ProvidePlugin({
                $: "jquery",
                jQuery: "jquery",
                Popper: "popper.js/dist/umd/popper",
                BpmnJS: ""
            })
        ],

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
                },

                //FONTS
                { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: "url-loader?limit=100000" }

                //FONTS AND FILES
                //{
                //    test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
                //    use: [
                //        {
                //            loader: 'file-loader',
                //            options: {
                //                name: '[name].[ext]',
                //                outputPath: 'fonts/'
                //            }
                //        }
                //    ]
                //}
            ]
        }
    }
];