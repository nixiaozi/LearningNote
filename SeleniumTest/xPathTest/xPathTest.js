// 下面的链接是这个函数的核心document.evalute的使用方法
// https://developer.mozilla.org/en-US/docs/Web/API/Document/evaluate
function xpathTest(str = "/html/body//*[text()]") {
    var headings = document.evaluate(str, document, null, XPathResult.ANY_TYPE, null);
    /* Search the document for all h2 elements.
     * The result will likely be an unordered node iterator. */
    var thisHeading = headings.iterateNext();
    var alertText = "selected nodes in this document are:\n";
    var count = 0;
    while (thisHeading) {

        alertText += thisHeading.textContent.trim() + "\n";
        thisHeading = headings.iterateNext();
        count++
    }
    // alert(alertText); // Alerts the text of all Selected elements
    // alert("总共找到了" + count + "个元素");
    console.info(alertText);
}

// chrome snaip version
function xpathTest(str = "/html/body//*[text()]") {
    var headings = document.evaluate(str, document, null, XPathResult.ANY_TYPE, null);
    /* Search the document for all h2 elements.
     * The result will likely be an unordered node iterator. */
    var thisHeading = headings.iterateNext();
    var alertText = "selected nodes in this document are:\n";
    var count = 0;
    while (thisHeading) {
        console.log(thisHeading);
        alertText += thisHeading.textContent.trim() + "\n";
        thisHeading = headings.iterateNext();
        count++
    }
    // alert(alertText); // Alerts the text of all Selected elements
    alert("总共找到了" + count + "个元素");
    //console.info(alertText);
}


xpathTest("/html/body//*[text()]") // 获取html-- body 节点下的所有文本内容
xpathTest("/html/body//img")  // 获取body节点下的所有图片 img节点
xpathTest("//img")  // 获取页面中所有的img节点

xpathTest("//li/a") // 获取页面中所有的 父元素为li的a元素
xpathTest("//li")  // 获取页面中所有的li节点
xpathTest("//li[node()]")  // 获取页面中所有有子节点的 li 节点
xpathTest("//li[text()]")  // 获取页面中所有包含直接文本节点的li节点（有换行也算是有文字节点）
xpathTest("//li[@*]") // 获取页面中至少包含一个属性的li节点（包含有id name 这样的）
xpathTest("//*")   // 获取页面中所有的节点

xpathTest("//div | //a")  // 获取页面中所有的div 节点和 a 节点
xpathTest("/html/body/div/div[@id='head']")  // 获取 html -- body -- div -- div 并且这个div节点的id为head

xpathTest("//a/ancestor::div") // 先定位所有的a节点，然后再获取它们的所有先代div节点
xpathTest("//div/ancestor-or-self::div") // 找到所有div节点，然后再找出所有先代或自己是div节点的元素
xpathTest("/html/body/div/div[@id='head']//attribute::width") // 找到对应的div节点，然后再找到包含 attribute 子节点的元素
xpathTest("/html/body/div/div[@id='head']//img/attribute::width")  //找到对应的div节点，然后再找到它所包含的所有img图片节点，最后筛选出图片节点中有width属性的
xpathTest("//*[contains(@class,'we')]")  // 找到有节点的class属性 包含 we 文本的片段。







// 需要把Default Levels 修改为 All Level 才能够显示 Console.log 的内容
console.log("Hello Everyone!");

// 在Default Level 模式下 可以使用 console.info()
console.info("Default Level");