CKEDITOR.dialog.add('wechatnewsDialog', function (editor) {
    return {
        title: '多图文属性',
        minWidth: 400,
        minHeight: 200,
        contents: [
            {
                id: 'tab-basic',
                label: '基本设置',
                elements:
                    [
                        {
                            type: 'text',
                            id: 'title',
                            label: '标题',
                            validate: CKEDITOR.dialog.validate.notEmpty("标题不能为空！"),

                            setup: function (element) {
                                this.setValue(element.getAttribute("title"));
                            },

                            commit: function (element) {
                                element.setAttribute("title", this.getValue());
                            }
                        },
                        {
                            type: 'text',
                            id: 'content',
                            label: '内容',
                            validate: CKEDITOR.dialog.validate.notEmpty("内容不能为空！"),

                            setup: function (element) {
                                this.setValue(element.getText());
                            },

                            commit: function (element) {
                                element.setText(this.getValue());
                            }
                        }
                    ]
            }
        ],

        onShow: function () {

            var selection = editor.getSelection();
            var element = selection.getStartElement();
            if (element) {
                element = element.getAscendant('news', true);
            }
            if (!element || element.getName() != 'news') {
                element = editor.document.createElement('news');
                this.insertMode = true;
            }
            else {
                this.insertMode = false;
            }

            this.element = element;
            if (!this.insertMode) {
                this.setupContent(this.element);
            }
        },

        onOk: function () {
            var dialog = this;
            var news = this.element;
            this.commitContent(news);

            if (this.insertMode) {
                editor.insertElement(news);
            }
        }
    };
});