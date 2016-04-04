'use strict';

var paths = require('../options/paths.js');

var ts = require('gulp-typescript');
var sourcemaps = require('gulp-sourcemaps');
var ngAnnotate = require('gulp-ng-annotate');
var tsProject = ts.createProject(paths.src.typescript_tsconfig);

module.exports = function (gulp) {
    gulp.task('ts:transpile', function () {
        var tsResult = gulp.src(paths.src.typescript)
            .pipe(sourcemaps.init())
            .pipe(ts(tsProject));

        return tsResult.js
            .pipe(ngAnnotate())
            .pipe(sourcemaps.write())
            .pipe(gulp.dest(paths.dest.typescript));
    });
}