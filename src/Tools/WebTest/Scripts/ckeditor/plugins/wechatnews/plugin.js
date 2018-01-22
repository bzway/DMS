CKEDITOR.plugins.add('wechatnews', {
    icons: 'wechatnews',
    init: function (editor) {
        if (true) {
            CKEDITOR.dialog.add('wechatnewsDialog', this.path + 'dialogs/wechatnews.js');
            editor.addCommand('wechatnews', new CKEDITOR.dialogCommand('wechatnewsDialog'));

        } else {
            var cmd = {
                exec: function (editor) {
                    //todo
                    var content = 'test';
                    editor.insertHtml(content);
                }
            };
            editor.addCommand('wechatnews', cmd);
        }

        editor.ui.addButton('wechatnews', {
            label: '微信素材',
            command: 'wechatnews'
        });
    }
});