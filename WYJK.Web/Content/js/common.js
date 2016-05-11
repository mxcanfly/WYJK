;
/**
 * 设置全局弹出层对象
 */
window.layer = window.top.layer;
/**
 * 关闭刷新产生的弹出层
 */
if (window.top.config.refresh) {
    layer.close(window.top.config.refresh);
}
/**
 * 刷新当前页码
 * @returns {} 
 */
function refresh() {
    window.top.config.refresh = layer.load(1, {
        shade: [0.1, '#000']
    });
    //location.replace(location.href);
    window.location.reload(true);
}