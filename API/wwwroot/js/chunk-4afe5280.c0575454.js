(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-4afe5280"],{"394b":function(t,e,a){"use strict";a.r(e);var n=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-container",{staticClass:"d-flex align-center justify-center overflow-y-hidden",staticStyle:{height:"60vh"}},[a("v-card",{staticClass:"rounded-lg pa-6 pb-10"},[t.isConfirmed?a("div",{staticClass:"d-flex flex-column align-center"},[a("v-icon",{attrs:{size:"64",color:"green"}},[t._v("mdi-check-circle-outline")]),a("div",{staticClass:"text-h6 my-4"},[t._v("Địa chỉ email của bạn đã được xác nhận")]),a("v-btn",{staticClass:"rounded-lg px-8",attrs:{depressed:"",height:"48",color:"rgba(254, 52, 100)",to:"/trang-chu"}},[a("div",{staticClass:"text-body-1 font-weight-bold white--text"},[t._v(" Về trang chủ ")])])],1):a("div",{staticClass:"d-flex flex-column align-center"},[a("v-icon",{attrs:{size:"64",color:"red"}},[t._v("mdi-alert-circle-outline")]),a("div",{staticClass:"text-h6 my-4"},[t._v("Địa chỉ email của bạn chưa được xác nhận")]),a("v-btn",{staticClass:"rounded-lg px-8",attrs:{depressed:"",height:"48",color:"rgba(254, 52, 100)",to:"/trang-chu"}},[a("div",{staticClass:"text-body-1 font-weight-bold white--text"},[t._v(" Về trang chủ ")])])],1)])],1)},r=[],i=a("c7eb"),c=a("1da1"),s={data:function(){return{isConfirmed:!1,token:""}},created:function(){this.token=this.$route.query.token,this.confirmEmail()},methods:{confirmEmail:function(){var t=this;return Object(c["a"])(Object(i["a"])().mark((function e(){var a;return Object(i["a"])().wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.prev=0,e.next=3,t.$http.get("/auths/me/confirm-email",{token:t.token});case 3:a=e.sent,a.success&&(t.isConfirmed=!0),e.next=10;break;case 7:e.prev=7,e.t0=e["catch"](0),console.log(e.t0);case 10:case"end":return e.stop()}}),e,null,[[0,7]])})))()}}},o=s,d=a("2877"),l=a("6544"),u=a.n(l),f=a("8336"),h=a("b0af"),v=a("a523"),p=a("132d"),b=Object(d["a"])(o,n,r,!1,null,null,null);e["default"]=b.exports;u()(b,{VBtn:f["a"],VCard:h["a"],VContainer:v["a"],VIcon:p["a"]})},a523:function(t,e,a){"use strict";a("4de4"),a("d3b7"),a("b64b"),a("2ca0"),a("99af"),a("20f6"),a("4b85");var n=a("e8f2"),r=a("d9f7");e["a"]=Object(n["a"])("container").extend({name:"v-container",functional:!0,props:{id:String,tag:{type:String,default:"div"},fluid:{type:Boolean,default:!1}},render:function(t,e){var a,n=e.props,i=e.data,c=e.children,s=i.attrs;return s&&(i.attrs={},a=Object.keys(s).filter((function(t){if("slot"===t)return!1;var e=s[t];return t.startsWith("data-")?(i.attrs[t]=e,!1):e||"string"===typeof e}))),n.id&&(i.domProps=i.domProps||{},i.domProps.id=n.id),t(n.tag,Object(r["a"])(i,{staticClass:"container",class:Array({"container--fluid":n.fluid}).concat(a||[])}),c)}})},e8f2:function(t,e,a){"use strict";a.d(e,"a",(function(){return r}));a("498a"),a("99af"),a("4de4"),a("d3b7"),a("b64b"),a("2ca0"),a("a15b");var n=a("2b0e");function r(t){return n["a"].extend({name:"v-".concat(t),functional:!0,props:{id:String,tag:{type:String,default:"div"}},render:function(e,a){var n=a.props,r=a.data,i=a.children;r.staticClass="".concat(t," ").concat(r.staticClass||"").trim();var c=r.attrs;if(c){r.attrs={};var s=Object.keys(c).filter((function(t){if("slot"===t)return!1;var e=c[t];return t.startsWith("data-")?(r.attrs[t]=e,!1):e||"string"===typeof e}));s.length&&(r.staticClass+=" ".concat(s.join(" ")))}return n.id&&(r.domProps=r.domProps||{},r.domProps.id=n.id),e(n.tag,r,i)}})}}}]);
//# sourceMappingURL=chunk-4afe5280.c0575454.js.map