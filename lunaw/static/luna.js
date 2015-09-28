var ws;

function connect(){
    ws = new WebSocket("ws://"+window.location.hostname+":8888/ws");
    ws.onopen = function() {
            ws.send("hello");
    };
    ws.onmessage = function (evt) {
            $('body')[0].style.backgroundColor = evt.data;
    };
    ws.onerror = function (evt) {
            alert("ERROR");
    };
    ws.onclose = function() {
            alert("CLOSE");
    };
}

$(function() {
    $('#modes a').click(function (e) {
        e.preventDefault()
        $(this).tab('show')
    });

    var color_changed = false;
    var color;
    setInterval(function () {
        if(color_changed) {
            ws.send(color);
            color_changed = false;
        }
    }, 40);

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

    connect();
});
