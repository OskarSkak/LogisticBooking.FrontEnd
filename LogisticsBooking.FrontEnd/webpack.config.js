const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");

const bundleFileName1 = 'bundle';
const dirName = 'wwwroot/dist';


module.exports = (env, argv) => {
    return {
        mode: argv.mode === "production" ? "production" : "development",
        entry: {'style' : './src/sass/app.scss',
             'dd' : './src/index.js',
            'datableCustom' : './src/datatableCustom.js'
        },
        output: {
            filename: '[name]' + '.js',
            path: path.resolve(__dirname, dirName)
        },
        module: {
            rules: [
                {
                    test: /\.s[c|a]ss$/,
                    use:
                        [
                            'style-loader',
                            MiniCssExtractPlugin.loader,
                            'css-loader',
                            {
                                loader: 'postcss-loader',
                                options: {
                                    config: {
                                        ctx: {
                                            env: argv.mode
                                        }
                                    }
                                },
                                options: {
                                    plugins: () => [require('autoprefixer')({
                                        'overrideBrowserslist': ['last 2 versions', 'ie 8', 'ie 9']
                                    })],
                                }
                            },
                            'sass-loader'
                        ]
                },
            ]
        },
        plugins: [
            new CleanWebpackPlugin(),
            new FixStyleOnlyEntriesPlugin(),
            new MiniCssExtractPlugin({
                filename: "[name].css",
                chunkFilename: "[name].css"
            })
        ]
    };
};

