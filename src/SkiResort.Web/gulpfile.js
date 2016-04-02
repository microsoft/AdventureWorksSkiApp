/// <binding BeforeBuild='default' Clean='clean' />

require('es6-promise').polyfill();

var gulp = require('gulp');

require('./gulp/tasks/copy.js')(gulp);
require('./gulp/tasks/lint.js')(gulp);
require('./gulp/tasks/preview.js')(gulp);
require('./gulp/tasks/sass.js')(gulp);
require('./gulp/tasks/watch.js')(gulp);
require('./gulp/tasks/typescript.js')(gulp);
require('./gulp/tasks/cordova.js')(gulp);

gulp.task('default', ['sass', 'copy', 'ts:transpile']);
gulp.task('dev', ['sass', 'copy', 'lint','ts:transpile', 'watch', 'preview']);