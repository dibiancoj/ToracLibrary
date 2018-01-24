"use strict";

//webpack 
//webpack --env.production=true -p

const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const webpack = require('webpack');

module.exports = env => {

    const isProduction = env && env.production ? true : false;
    console.log('Production: ', isProduction);

    return {

        entry: {
            Entry1: './App/MyEntryPoint1.js',
            Entry2: './App/MyEntryPoint2.js',
        },

        output: {
            filename: './wwwroot/Build/App/[name].Bundle.js'
        },

        devtool: isProduction ? 'none' : 'source-map',

        module: {
            rules: [
                {
                    test: /\.js$/,
                    exclude: /(node_modules|bower_components)/,
                    use: {
                        loader: 'babel-loader',
                        options: {
                            presets: ['babel-preset-env']
                        }
                    }
                }
            ]
        },

        plugins:
        [
          //this just cleans up the output folder so we don't have left over items that shouldn't be there.
            new CleanWebpackPlugin([path.join(__dirname, '/wwwroot/Build/App/*.*')]), //removes all files in this directory. Or you can do '/wwwroot/Build/**.js' if you want to delete just the js files

            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/Build/Framework/vendor-manifest.json')
            })

            //if i don't want to import jquery on every screen i can do this and this will make jquery global
            //new webpack.ProvidePlugin({
            //    $: "jquery",
            //    jquery: "jquery",
            //    "window.jQuery": "jquery",
            //    jQuery: "jquery"
            //}),

          new webpack.DllReferencePlugin({
                context: '.',
                manifest: require('./wwwroot/Build/Framework/vendor-manifest.json'),
            })
        ]
    };
};