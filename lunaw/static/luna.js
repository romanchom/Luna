$(function() {
    //Set up tabs
    $('#modes a').click(function (e) {
        e.preventDefault()
        $(this).tab('show')
    });

    var color_changed = false;
    var color;
    setInterval(function () {
        if(color_changed) {
            $.getJSON($SCRIPT_ROOT + '/_manual_color', {
                color: color
            }, function(data) {
                $('body')[0].style.backgroundColor = data.result;
            });
            color_changed = false;
        }
    }, 100);

    $('#colpick').colorpicker({
        customClass: 'colorpicker-2x',
        sliders: {
            saturation: {
                maxLeft: 200,
                maxTop: 200
            },
            hue: {
                maxTop: 200
            },
            alpha: {
                maxTop: 200
            }
        }
    }).on('changeColor.colorpicker', function(event){
        color_changed = true;
        color = event.color.toHex();
    });
});