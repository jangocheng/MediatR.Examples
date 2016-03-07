﻿module.exports = function () {
    var root = './';
    var clientApp = root + 'app/';
    var build = './build/';
    var specRunnerFile = 'specRunner.html';
    var translations = 'common/translations/';
    var bower = {
        json: require('./bower.json'),
        directory: './libs/',
        ignorePath: ''
    };

    var config = {
        index: 'index.html',
        specRunner: root + specRunnerFile,
        bower: bower,
        root: root,
        app: clientApp,
        js: {
            all: clientApp + '**/*.js',
            maps: clientApp + '**/*.js.map',
            appFiles: [
                clientApp + '**/*.js',
                '!' + clientApp + '**/*.spec.js',
                '!' + clientApp + '**/*.mock.js'
            ],
            appFilesToTest: [
                clientApp + '**/*.js',
                '!' + clientApp + '**/*.spec.js',
                '!' + clientApp + '**/*.mock.js',
                '!' + clientApp + '**/*.route.js',
                '!' + clientApp + '**/*.config.js',
            ],
            specsAndMocks: [
                clientApp + '**/*.spec.js',
                clientApp + '**/*.mock.js'
            ],
            order: [
                '**/app.module.js',
                '**/*.module.js',
                '**/*.js'
            ]
        },
        ts: {
            allTs: clientApp + '**/*.ts',
            libTypingsAllTs: root + 'typings/main/**/*.ts'
        },
        build: {
            output: build,
            temp: './.tmp/',
            templateCache: {
                file: 'templates.js',
                options: {
                    module: 'app',
                    standAlone: false,
                    root: 'app/'
                }
            },
            htmlTemplatesSrc: clientApp + '**/*.html',
            translationsDest: build + translations,
            staticJsonFiles: [
                translations + '*.json', // translation
                'app.json',  // tokens
            ],
            staticAppFiles: [
                root + 'wwwroot.nuspec', 
                root + '_web.config',  
            ],
        },
        vendors: []
    }

    /**
     * wiredep and bower settings
     */
    config.getWiredepDefaultOptions = function () {
        var options = {
            bowerJson: config.bower.json,
            directory: config.bower.directory,
            ignorePath: config.bower.ignorePath
        };

        return options;
    };

    return config;
}