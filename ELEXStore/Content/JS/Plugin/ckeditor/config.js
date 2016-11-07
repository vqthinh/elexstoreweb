/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'vi';
    config.filebrowserBrowseUrl = '/Content/JS/Plugin/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Content/JS/Plugin/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/Content/JS/Plugin/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/Content/JS/Plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Data';
    config.filebrowserFlashUploadUrl = '/Content/JS/Plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, '/Content/JS/Plugin/ckfinder/');
};
