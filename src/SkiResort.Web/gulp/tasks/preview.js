'use strict';

var paths = require('../options/paths.js');

var express = require('express');

module.exports = function (gulp) {
    gulp.task('preview', function () {
        var app = express();
        app.use(express.static("./wwwroot"));
        app.listen(8080);
        console.log("Web server listening on http://127.0.0.1:8080");
    });
}