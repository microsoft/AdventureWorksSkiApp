'use strict';

var srcRoot = './src';
var sassRoot = srcRoot + '/scss';
var tsRoot = srcRoot + '/app';
var fontsRoot = srcRoot + '/fonts';
var icomoonRoot = srcRoot + '/icomoon';

var destRoot = './wwwroot';
var cssRoot = destRoot + '/css';
var jsRoot = destRoot + '/js';

function buildPath(packageName, _paths) {
    var paths = ['.', 'node_modules', packageName].concat(_paths);
    return paths.join('/');
};

module.exports = {
    src: {
        all: [destRoot + '/**/*.*', '!' + destRoot + '/web.config', '!' + destRoot + '/cordova.js'],
        fonts: [
            buildPath('ionic-sdk', ['release', 'fonts', '*.{eot,svg,ttf,woff}']),
            fontsRoot + '/*.{eot,svg,ttf,woff}'
        ],
        sass: [sassRoot + '/base/**/*.scss', sassRoot + '/components/**/*.scss', sassRoot + '/main.scss'],
        main_sass: sassRoot + '/main.scss',
        ionic_sass: sassRoot + '/ionic.scss',
        main_sass_watch: sassRoot + '/**/*.scss',
        index: srcRoot + '/index.html',
        libs: [
            buildPath('ionic-sdk', ['release', 'js', 'ionic.bundle.min.js']),
            buildPath('jquery', ['dist', 'jquery.min.js']),
            buildPath('lodash', ['lodash.min.js'])
        ],
        cordovaLibs: srcRoot + '/cordova/**/*.*',
        icomoon_style: icomoonRoot + '/style.css',
        icomoon_fonts: icomoonRoot + '/fonts/*.*',
        templates: tsRoot + '/modules/**/*.html',
        typescript: [
            tsRoot + '/app.ts',
            tsRoot + '/app.config.ts',
            tsRoot + '/**/*.module.ts',
            tsRoot + '/**/*.module.config.ts',
            tsRoot + '/**/models/**/*.ts',
            tsRoot + '/**/*.ts'
        ],
        typescript_lint: tsRoot + '/**/*.ts',
        typescript_tsconfig: tsRoot + '/tsconfig.json',
        sass_lint: 'scsslint.yml',
        css: cssRoot+ '/main.css',
        images: srcRoot + '/images/**/*.*',
        azureSearch: './AzureSearchSampleData/*.json'
    },
    dest: {
        sass: cssRoot,
        templates: jsRoot,
        index: destRoot,
        libs: jsRoot,
        js: './www/js/',
        typescript: jsRoot,
        fonts: destRoot + '/fonts/',
        cordova: '../SkiResort.Mobile/www/',
        icomoon_style: cssRoot,
        icomoon_fonts: destRoot + '/fonts/',
        images: destRoot + '/images/',
        azureSearch: destRoot + '/azureSearch/'
    }
};