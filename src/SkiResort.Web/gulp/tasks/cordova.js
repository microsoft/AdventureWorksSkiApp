'use strict';

var paths = require('../options/paths.js');
var runSequence = require('run-sequence');

module.exports = function (gulp) {
    
    gulp.task('cordova:copy', function () {
        gulp.src(paths.src.all)
            .pipe(gulp.dest(paths.dest.cordova));
    });
    
    gulp.task('cordova', function() {
        runSequence('default', 'cordova:copy');
    });
};