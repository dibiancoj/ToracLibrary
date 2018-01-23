//webpack --config webpack.config.vendor.js
//webpack --config webpack.config.vendor.js --env.production=true -p

const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = env => {

    const isProduction = env && env.production ? true : false;
    console.log('Production: ', isProduction);

    const extractCSS = new ExtractTextPlugin('./wwwroot/Build/Framework/vendor.css');

    return {

        entry: {
            Vendor: ['jquery', 'bootstrap', 'bootstrap/dist/css/bootstrap.css'],
        },
        output: {
            filename: './wwwroot/Build/Framework/vendor.bundle.js',
            library: 'vendor_lib',
        },
        module: {
            loaders: [
                //css for bootstrap (or any css) import
                {
                    test: /\.css(\?|$)/,
                    use: extractCSS.extract({ use: !isProduction ? 'css-loader' : 'css-loader?minimize' })
                },
                {
                    test: /.(ttf|otf|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
                    use: [{
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                            outputPath: './wwwroot/Build/Framework/Fonts/'    // where the fonts will go
                        }
                    }]
                },
            ]
        },
        plugins: [

            extractCSS, 

            //this just cleans up the output folder so we don't have left over items that shouldn't be there.
            new CleanWebpackPlugin([path.join(__dirname, '/wwwroot/Build/Framework/*.*')]), //removes all files in this directory. Or you can do '/wwwroot/Build/**.js' if you want to delete just the js files

            new webpack.DllPlugin({
                name: 'vendor_lib',
                path: './wwwroot/Build/Framework/vendor-manifest.json',
            }),

            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' })
            //new webpack.optimize.CommonsChunkPlugin({

            //    //with out any additional parameters everything gets ordered from right to left. So vendor 2 has all the web pack plumbing. Then vendor just has its own stuff.
            //    name: ["Vendor"],

            //    //*** if you get a jsonp error with entry point it means the entry point is being loaded before the common chunk (vendor)
            //    //(with more entries, this ensures that no other module
            //    //goes into the vendor chunk)
            //    minChunks: Infinity
            //})
        ]
    }
};