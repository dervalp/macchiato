describe( "Entity Service", function () {
  it( "should exist", function () {
    EntityService.should.exist;
  } );
  describe( "when I want to call a Post Controller", function () {
    describe( "I need to create a Service", function () {
      describe( "Given that service", function () {
        var postEntityService = new EntityService({
            url: "/sitecore/api/ssc/speak-blog-controllers/Post/"
        } );

        var cleanupDatabase = function ( testStage, callback ) {
          var entitiesToDestroy = [];

          var destroyEntities = function () {

            if ( entitiesToDestroy.length === 0 ) {
              callback();
              return;
            }

            var entityToDestroy = entitiesToDestroy.shift();

            entityToDestroy.destroy().then( destroyEntities ).fail( destroyEntities );

          }

          postEntityService.fetchEntities().option( "binding", false ).execute().then( function ( entities ) {

            entities.forEach( function ( entity ) {
              if ( entity.Title.substring( 0, 4 ).toLowerCase() === "test" ) {
                entitiesToDestroy.push( entity );
              }
            } );

            destroyEntities();

          } ).fail( callback );

        };

        var failedTest = function () {
          throw "Test is failing";
        };

        before( function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          cleanupDatabase( "Pre-test", done );
        } );

        after( function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          cleanupDatabase( "Post-test", done );
        } );

        it( "should be able to list all entities", function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          postEntityService.fetchEntities( {} ).execute().then( function ( posts ) {
            done();
          } );
        } );

        it( "should be able to create an entity", function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          postEntityService.create( {
            Title: "Test 1 This is a title",
            Content: "This is some html content without html"
          } ).execute().then( function ( post ) {

            // Ensure that a ID exists for post
            post.Id.should.exist;

            done();
          } ).fail( done );
        } );

        it( "should be able to read new entity after it's been created", function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          postEntityService.create( {
            Title: "Test 2 This is a title",
            Content: "This is some html content without html"
          } ).execute().then( function ( post ) {

            postEntityService.fetchEntity( post.Id, {} ).execute().then( function ( newPost ) {

              newPost.Title.should.equal( "Test 2 This is a title" );
              done();

            } ).fail( done );

          } ).fail( done );
        } );

        it( "should fail to save an entity with validation issues", function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          postEntityService.create( {
            Title: "Test 3 This is a title",
            Content: "This is some html content without html"
          } ).execute().then( function ( post ) {

            post.Title = null;

            post.save().then( function () {
              done( new Error( "Should not have saved because the entity is invalid" ) );
            } ).fail( function ( error ) {
              console.log( error );
              done();
            } );

          } ).fail( done );

        } );

        it( "should be able to delete an entity that exist", function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          postEntityService.create( {
            Title: "Test 4 Post to delete",
            Content: "This is some html content without html"
          } ).execute().then( function ( post ) {

            post.Id.should.exist;
            post.destroy().then( function () {
              done();
            } ).fail( done );

          } ).fail( done );
        } );

        it( "should be not create an entity that already exists", function ( done ) {
          this.timeout( 5000 );
          this.slow( 3000 );
          postEntityService.create( {
            Title: "Test 5 This is a title",
            Content: "This is some html content without html"
          } ).execute().then( function ( post ) {

            // Ensure that a ID exists for post
            post.Id.should.exist;

            postEntityService.create( {
              Id: post.Id,
              Title: "Test 5 This is a title",
              Content: "This is some html content without html"
            } ).execute().then( function () {
              done( new Error( "Should be not create an entity that already exists" ) );
            } ).fail( function ( error ) {
              console.log( error );
              done();
            } );

          } ).fail( done );
        } );

      } );
    } );
  } );
  describe( "the custom validators", function () {

    it( "should automatically inject themselves into the page", function ( done ) {

      var postEntityService = new EntityService({
          url: "/sitecore/api/ssc/speak-blog-controllers/Post/"
      } );

      postEntityService.create().then( function ( postEntity ) {

        postEntity.isValid().should.be.false;
        postEntity.Title = "Lorem ipsum";
        postEntity.isValid().should.be.false;
        postEntity.AuthorEmail = "abc@abc";
        postEntity.isValid().should.be.false;
        postEntity.AuthorEmail = "abc@abc.com";
        postEntity.isValid().should.be.true;

        done();

      } ).fail( done );

    } );

  } );
  describe( "I want to ensure when array datatypes are specified they", function () {

    it( "should sanitize gracefully", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var speakSampleSpeakTestEntityService = new EntityService({
          url: "/sitecore/api/ssc/speak-sample/speaktest"
      } );

      speakSampleSpeakTestEntityService.fetchEntities().execute().then( function ( entities ) {

        entities[ 0 ].RelatedIds.length.should.be.greaterThan( 0 );
        entities[ 0 ].RelatedIds.forEach( function ( guid ) {
          EntityService.utils.guid.isValid(guid).should.be.true;
        } );

        entities[ 0 ].RelatedNumbers.length.should.be.greaterThan( 0 );
        entities[ 0 ].RelatedNumbers.forEach( function ( number ) {
          number.should.be.a.Number;
        } );

        entities[ 0 ].RelatedStrings.length.should.be.greaterThan( 0 );
        entities[ 0 ].RelatedStrings.forEach( function ( string ) {
          string.should.be.a.String;
        } );

        entities[ 1 ].RelatedIds.should.have.a.lengthOf( 0 );
        entities[ 1 ].RelatedNumbers.should.have.a.lengthOf( 0 );
        entities[ 1 ].RelatedStrings.should.have.a.lengthOf( 0 );

        should.equal( entities[ 2 ].RelatedIds, null );
        should.equal( entities[ 2 ].RelatedNumbers, null );
        should.equal( entities[ 2 ].RelatedStrings, null );

        entities.forEach( function ( entity ) {
          entity.isValid().should.be.true;
        } );

        done();

      } ).fail( done );

    } );

  } );
} );