//*****************************************************************************************                                      
//Plug In: Native Drag And/ /Drop                                                         *
//Version: 1.0 - 8/6/2016                                                                 *
//*****************************************************************************************

//Version 1.0 - 8/6/2016 - Initial plug in created

//Notes:
//1.plug in issue - if you start dragging then go off of the browser window we don't have an event. I tried to add document.dragleave or body and it just conflicts too much

//if you want to wrap it in jquery.ready (like any other plug in please use the following)
//we need to pass in the global jquery object so we can reference public methods. So we create a self invoking function and pass in jquery which has access to the drag and drop plug in
//(function (_$) {

//    //document ready (we have access to the global jquery scope. So we use that instead of $
//    $(document).ready(function () {

//        _$('.FinFormsDrag').DragAndDrop({
//            OnDocumentOnStartDrag: function (element) {
//                $('.FinFormsDrag').show();
//            },
//            OnDragOver: function (element) {
//                $('.FinFormsDrag').addClass('FinFormsDragOver');
//            },
//            OnDragLeave: function (element) {
//                $('.FinFormsDrag').removeClass('FinFormsDragOver');
//            },
//            OnDropSuccess: function (element, e, files) {
//                alert('success');
//                $('.FinFormsDrag').hide();
//            },
//            OnDropFailure: function (element) {
//                $('.FinFormsDrag').hide();
//            }
//        });
//    });
//}($));

//parameters
//OnDocumentOnStartDrag: function(plugInElement){ }
//OnDragOver: function(plugInElement) { }
//OnDragLeave: function(plugInElement) { }
//OnDropSuccess: function(plugInElement,evt, files){ }
//OnDropFailure: function(plugInElement){ }

//<div class="col-xs-12 FinFormsDrag">Drag the file you wish to upload here</div>

/*
    .FinFormsDrag {
        display: none;
        font-weight: bold;
        text-align: center;
        padding: 1em 0;
        margin: 1em 0;
        color: #555;
        border: 2px dashed #555;
        border-radius: 7px;
        cursor: default;
    }

    .FinFormsDragOver {
        color: #2e81d1;
        border-color: #2e81d1;
        border-style: solid;
    }
 */

//You need an anonymous function to wrap around your function to avoid conflict
(function ($) {

    //use a variable so we don't clutter the fn namespace
    var methods = {

        //initialize method
        DragAndDrop: function (parameters) {

            //grab the parameters into a closure
            var params = parameters;

            //Iterate over the current set of matched elements
            return this.each(function () {

                //throw this into a variable to grab onto later
                var _this = $(this);

                //bind the on start drag
                $(document).on('dragstart dragover dragenter', function (evt) {

                    //stop propagation
                    evt.preventDefault();
                    evt.stopPropagation();

                    //show the container so the user knows how to drag
                    params.OnDocumentOnStartDrag(_this);
                }).on('drop', function (evt) {

                    //for bad drops

                    //prevent the default from bubbling up
                    evt.preventDefault();

                    //mis drop, hide the target
                    params.OnDropFailure(_this);
                });

                //on drag over the target & on drag enter
                $(this).on('dragover dragenter', function (evt) {

                    //on drag over
                    params.OnDragOver(_this);

                }).on('dragleave dragend drop', function (evt) {

                    //on drag leave
                    params.OnDragLeave(_this);

                }).on('drop', function (evt) {

                    //call the on drop
                    params.OnDropSuccess(_this, evt, evt.originalEvent.dataTransfer.files);
                });
            });
        }
    };

    //add the numeric plug in 
    $.fn.DragAndDrop = function (method) {

        // Method calling logic
        if (methods[method]) {
            //run the method
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));

        } else if (typeof method === 'object' || !method) {
            //default to the base method
            return methods.DragAndDrop.apply(this, arguments);

        } else {
            //throw an error because we can't find the method
            $.error('Method ' + method + ' does not exist on jQuery.DragAndDrop');
        }
    };
})(jQuery);