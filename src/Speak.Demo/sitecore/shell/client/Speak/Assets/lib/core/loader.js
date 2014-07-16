!function(e){"object"==typeof exports?module.exports=e():"function"==typeof define&&define.amd?define(e):"undefined"!=typeof window?window.SPEAKloader=e():"undefined"!=typeof global?global.SPEAKloader=e():"undefined"!=typeof self&&(self.SPEAKloader=e())}(function(){var define,module,exports;return (function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);throw new Error("Cannot find module '"+o+"'")}var f=n[o]={exports:{}};t[o][0].call(f.exports,function(e){var n=t[o][1][e];return s(n?n:e)},f,f.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
( function ( global ) {
    var server = "",
        cache = {},
        scripts = [],
        isBrowser = ( typeof window !== "undefined" ),
        toArray = Array.prototype.slice; //need to check cross browser

    win = isBrowser ? window : global,

    scripts = isBrowser ? toArray.call( win.document.scripts ) : []; //need to check cross browser
    /**
     * the main loader function
     * @param {id} config key
     * @param {cb} callback
     */

    function loader( id, cb ) {
        //reset the errors
        loader.errors = void 0;
        requestConfig( id, function ( err ) {
            cleanup();
            cb( err );
        } );
    }

    /**
     * cleanup, reset all the server created values when
     * the script finishes
     */

    function cleanup() {
        loader.errors = void 0;
        loader.loader = void 0;
        loader.config = void 0;
    }

    /**
     * read data attributes on the script tags
     */

    function processDataAttributes() {
        var id, app, ready, deferred;

        //convenience method
        toArray.call( scripts[ scripts.length - 1 ].attributes )
            .forEach( function ( attribute ) {
                if ( attribute.nodeName === "data-loader-id" ) {
                    id = attribute.nodeValue;
                }
                if ( attribute.nodeName === "data-loader-app" ) {
                    app = attribute.nodeValue;
                }
                if ( attribute.nodeName === "data-loader-ready" ) {
                    ready = attribute.nodeValue;
                }
            } );
        //TODO add regex for filename
        if ( id && id.length > 0 ) {
            //okay we have an id and a filename
            //now we request the id, and then attach the
            //app file to the DOM as a callback
            requestConfig( id, function ( err ) {
                cleanup();
                if ( app && app.length > 3 ) {
                    if ( err ) {
                        throw err;
                    } else {
                        attachScript( {
                            url: app + ".js"
                        }, function () {} );
                    }
                }
                if ( ready ) {
                    window[ ready ].call( window, err );
                }
            } );
        }
    }

    /**
     * send a request for the config file
     */

    function requestConfig( id, cb ) {
        attachScript( {
            url: server + id + ".js"
        }, function () {
            //we check for errors first
            if ( typeof loader.errors !== "undefined" ) {
                return cb( loader.errors );
            }
            //after the script is attached
            //we need to determine if it's prod or dev
            determineMode( cb );
        } );
    }

    /**
     * determines if the script returned is a
     * dev or prod configuration
     * @param {cb} callback
     */

    function determineMode( cb ) {
        //the loader will be null if
        //production, a function if in dev
        //and undefined if there was a problem
        var loaderConfig = SPEAKloader.config;

        if ( loaderConfig === void 0 ) {
            //production
            cb();
        } else if ( loaderConfig.scripts && loaderConfig.scripts instanceof Array ) {
            //dev
            var scripts = loaderConfig.scripts;
            //if there are no scripts in the config
            //we can pass to the callback
            if ( scripts.length === 0 ) {
                return cb();
            }
            //set everything to not loaded
            setCacheAndDeps( scripts, cb );
            //we process scripts in the zero-ith position
            //first.  These are non-dependent scripts
            for ( var i = 0; i < scripts[ 0 ].length; i++ ) {
                var url = scripts[ 0 ][ i ];
                //if it isn't in cache, or isn't loaded load it
                if ( !( cache[ url ] && cache[ url ].loaded ) ) {
                    attachScript( {
                        url: url + ".js"
                    }, scriptIsLoaded( url, null, null, cb ) );
                } else {
                    //otherwise mark done and see if all done
                    scriptIsLoaded( url, null, null, cb )();
                }
            }
            if ( scripts.length > 1 ) {
                //now we load the rest in priority sequence
                //we start at 0 because the first call
                //will increment to 1.
                var currScript = 0;
                getDependencies( function ( test ) {
                    if ( test ) {
                        return typeof scripts[ currScript + 1 ] !== "undefined";
                    }
                    currScript++;
                    if ( currScript >= scripts.length ) {
                        return null;
                    }
                    return scripts[ currScript ];
                }, cb );
            }
        } else {
            //error handler, bad script content
            cb( new Error( "invalid dependency config" ) );
        }
    }

    function setCacheAndDeps( scripts, cb ) {
        var depChain = [];
        for ( var l = 0; l < scripts.length; l++ ) {
            if ( scripts[ l ].length === 0 ) {
                return cb( new Error( "Empty dependency array." ) );
            }
            if ( l > 0 ) {
                [].push.apply( depChain, scripts[ l ] );
            }
            for ( var r = 0; r < scripts[ l ].length; r++ ) {
                if ( scripts[ l ][ r ].length === 0 ) {
                    return cb( new Error( "Empty dependency url." ) );
                }
                if ( !cache[ scripts[ l ][ r ] ] ) {
                    cache[ scripts[ l ][ r ] ] = {
                        loaded: false
                    };
                }
                if ( l > 0 ) {
                    cache[ scripts[ l ][ r ] ].deps = depChain.slice( 0 );
                } else {
                    cache[ scripts[ l ][ r ] ].deps = scripts[ l ].slice( 0 );
                }
            }
        }
    }

    /**
     * returns a callback that checks a loading list to see if a script is loaded.
     * @param {orderedCb} callback if ordered loading is being done
     * @param {nextScript} factory for next priority level
     * @param {cb} the done callback
     */

    function scriptIsLoaded( url, orderedCb, nextScript, cb ) {
        return function () {
            cache[ url ].loaded = true;
            if ( depsLoaded( url ) ) {
                if ( allLoaded() ) {
                    cb();
                } else if ( orderedCb ) {
                    //if it's ordered we need to
                    //traverse down to the next
                    //priority level
                    orderedCb( nextScript, cb );
                }
            }
        };
    }

    /**
     * check if depdencies are loaded in cache
     */

    function depsLoaded( key ) {
        var deps = cache[ key ].deps;
        for ( var i = 0; i < deps.length; i++ ) {
            if ( !cache[ deps[ i ] ].loaded ) {
                return false;
            }
        }
        return true;
    }

    /**
     * check if depdencies are loaded in cache
     */

    function allLoaded() {
        var cached = Object.keys( cache );
        for ( var i = 0; i < cached.length; i++ ) {
            if ( !cache[ cached[ i ] ].loaded ) {
                return false;
            }
        }
        return true;
    }

    /**
     * ordered loading callback
     * @param {nextScript} factory for priority level
     * @param {cb} the done callback
     */

    function ifOrdered( nextScript, cb ) {
        var currScript = nextScript( true );
        if ( currScript ) {
            getDependencies( nextScript, cb );
        } else {
            cb( new Error( "Error in dependency chain." ) );
        }
    }

    /**
     * load dependencies
     * @param {nextScript} factory for priority level
     * @param {cb} the done callback
     */

    function getDependencies( nextScript, cb ) {
        var currScript = nextScript();
        for ( var ds = 0; ds < currScript.length; ds++ ) {
            var url = currScript[ ds ];
            if ( !( cache[ url ] && cache[ url ].loaded ) ) {
                attachScript( {
                    url: url + ".js"
                }, scriptIsLoaded( url, ifOrdered, nextScript, cb ) );
            } else {
                scriptIsLoaded( url, ifOrdered, nextScript, cb )();
            }
        }
    }

    /**
     * Attaches a script to the DOM and executes it.
     * @param {options} object, normally with .url
     * @param {cb} callback, called when script is loaded
     */

    function attachScript( options, cb ) {
        var script = document.createElement( "script" );

        script.type = "text/javascript";
        script.src = options.url;
        script.onload = cb;
        script.onerror = scriptLoadError;

        ( document.head || document.getElementsByTagName( "head" )[ 0 ] )
            .appendChild( script );
    }

    /**
     * Handling script loading errors
     */

    function scriptLoadError() {
        //need error handling
        loader.errors = new Error( "script loading error." );
    }

    function requestLoad( scripts, cb ) {
        //if there are no scripts in the config
        //we can pass to the callback
        if ( scripts.length === 0 ) {
            return cb();
        }
        //set everything to not loaded
        setCacheAndDeps( scripts, function () {} );
        //we process scripts in the zero-ith position
        //first.  These are non-dependent scripts
        for ( var i = 0; i < scripts[ 0 ].length; i++ ) {
            var url = scripts[ 0 ][ i ];
            //if it isn't in cache, or isn't loaded load it
            if ( !( cache[ url ] && cache[ url ].loaded ) ) {
                attachScript( {
                    url: url + ".js"
                }, scriptIsLoaded( url, null, null, cb ) );
            } else {
                //otherwise mark done and see if all done
                scriptIsLoaded( url, null, null, cb )();
            }
        }
        if ( scripts.length > 1 ) {
            //now we load the rest in priority sequence
            //we start at 0 because the first call
            //will increment to 1.
            var currScript = 0;
            getDependencies( function ( test ) {
                if ( test ) {
                    return typeof scripts[ currScript + 1 ] !== "undefined";
                }
                currScript++;
                if ( currScript >= scripts.length ) {
                    return null;
                }
                return scripts[ currScript ];
            }, cb );
        }
    }

    global.use = function ( arr, cb ) {
        requestLoad( arr, cb );
    };
    //exporting loader
    global.loader = loader;

    //check for data attributes and start if they exist
    if ( isBrowser ) {
        processDataAttributes();
    }
}( this ) );
},{}]},{},[1])
(1)
});
;