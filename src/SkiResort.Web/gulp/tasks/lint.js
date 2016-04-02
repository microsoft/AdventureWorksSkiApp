'use strict';

var paths = require('../options/paths.js');
var tslint = require('gulp-tslint');
var scsslint = require('gulp-scss-lint');

module.exports = function (gulp) {
    gulp.task('lint:ts', function () {
        return gulp.src(paths.src.typescript_lint)
            .pipe(tslint())
            .pipe(tslint.report('prose', {
                emitError: false,
                summarizeFailureOutput: true
            }));
    });
    
    gulp.task('lint:sass', function () {
        return gulp.src(paths.src.sass)
            .pipe(scsslint({ 'config': paths.src.sass_lint }));
    });

    gulp.task('lint', ['lint:ts', 'lint:sass']);
};