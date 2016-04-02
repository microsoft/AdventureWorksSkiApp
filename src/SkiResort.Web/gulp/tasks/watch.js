'use strict';

var paths = require('../options/paths.js');

module.exports = function (gulp) {
    gulp.task('watch', function () {
        gulp.watch(paths.src.main_sass_watch, ['sass:main', 'lint:sass']);
        gulp.watch(paths.src.ionic_sass, ['sass:ionic']);
        gulp.watch(paths.src.index, ['copy:index']);
        gulp.watch(paths.src.templates, ['copy:templates']);
        gulp.watch(paths.src.typescript, ['lint:ts', 'ts:transpile']);
    });
}
