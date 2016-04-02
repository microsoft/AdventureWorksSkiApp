'use strict';

var paths = require('../options/paths.js');
var templateCache = require('gulp-angular-templatecache');
var rename = require('gulp-rename');

module.exports = function (gulp) {
    
    gulp.task('copy:index', function () {
        gulp.src(paths.src.index)
            .pipe(gulp.dest(paths.dest.index));
    });
    
    gulp.task('copy:images', function () {
        gulp.src(paths.src.images)
            .pipe(gulp.dest(paths.dest.images));
    });
    
    gulp.task('copy:templates', function () {
        gulp.src(paths.src.templates)
            .pipe(templateCache({
                filename: 'app-tpl.js',
                module: 'app'
            }))
            .pipe(gulp.dest(paths.dest.templates));
    });
    
    gulp.task('copy:fonts', function () {
        gulp.src(paths.src.fonts)
            .pipe(gulp.dest(paths.dest.fonts));
    });
    
    gulp.task('copy:icomoon', function () {
        gulp.src(paths.src.icomoon_fonts)
            .pipe(gulp.dest(paths.dest.icomoon_fonts));
    });
    
    gulp.task('copy:libs', function () {
        gulp.src(paths.src.libs)
            .pipe(gulp.dest(paths.dest.libs));
    });

    gulp.task('copy:cordovaLibs', function () {
        gulp.src(paths.src.cordovaLibs)
            .pipe(gulp.dest(paths.dest.index));
    });
    
    gulp.task('copy', ['copy:index', 'copy:images', 'copy:templates', 'copy:fonts', 'copy:icomoon', 'copy:libs', 'copy:cordovaLibs']);
};