<template>
    <h1>Hello!</h1>
</template>

<script>
    import Vue from "vue"; // 需要添加Vue 引用，否则会出现 new Vue() 时 'Vue' is not defined  no-undef 的错误
    // 我们的数据对象
    var data = { a: 1 }

    // 该对象被加入到一个 Vue 实例中
    var vm = new Vue({
        data: data
    })

    // 获得这个实例上的 property
    // 返回源数据中对应的字段
    vm.a == data.a // => true
    console.info("vm.a == data.a is:" + (vm.a == data.a));

    // 设置 property 也会影响到原始数据
    vm.a = 2
    data.a // => 2
    console.info("data.a is:" + data.a + ";after vm.a = 2");

    // ……反之亦然
    data.a = 3
    vm.a // => 3
    console.info("vm.a is:" + vm.a + "; after data.a = 3");
</script>