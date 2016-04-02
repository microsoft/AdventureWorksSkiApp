'use strict';

var paths = require('../options/paths.js');

var sourcemaps = require('gulp-sourcemaps');
var sass = require('gulp-sass');
var minifyCss = require('gulp-minify-css');
var rename = require('gulp-rename');
var autoprefixer = require('gulp-autoprefixer');

module.exports = function (gulp) {
    
    gulp.task('sass:main', function (done) {
        gulp.src(paths.src.main_sass)
            .pipe(sourcemaps.init())
            .pipe(sass().on('error', sass.logError))
            .pipe(autoprefixer({
                browsers: ['last 2 versions'],
                cascade: false
            }))
            .pipe(gulp.dest(paths.dest.sass))
            .pipe(minifyCss({
                keepSpecialComments: 0
            }))
            .pipe(rename({ extname: '.min.css' }))
            .pipe(sourcemaps.write('./'))
            .pipe(gulp.dest(paths.dest.sass))
            .on('end', done);
    });
    
    gulp.task('sass:ionic', function (done) {
        gulp.src(paths.src.ionic_sass)
            .pipe(sourcemaps.init())
            .pipe(sass().on('error', sass.logError))
            .pipe(gulp.dest(paths.dest.sass))
            .pipe(minifyCss({
                keepSpecialComments: 0
            }))
            .pipe(rename({ extname: '.min.css' }))
            .pipe(sourcemaps.write('./'))
            .pipe(gulp.dest(paths.dest.sass))
            .on('end', done);
    });
    
    gulp.task('sass', ['sass:ionic', 'sass:main']);
}