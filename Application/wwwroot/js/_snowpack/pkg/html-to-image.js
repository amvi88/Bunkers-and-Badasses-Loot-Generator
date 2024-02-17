function k(){throw new Error("setTimeout has not been defined")}function U(){throw new Error("clearTimeout has not been defined")}var g=k,d=U,b;typeof window!="undefined"?b=window:typeof self!="undefined"?b=self:b={},typeof b.setTimeout=="function"&&(g=setTimeout),typeof b.clearTimeout=="function"&&(d=clearTimeout);function M(t){if(g===setTimeout)return setTimeout(t,0);if((g===k||!g)&&setTimeout)return g=setTimeout,setTimeout(t,0);try{return g(t,0)}catch(e){try{return g.call(null,t,0)}catch(n){return g.call(this,t,0)}}}function et(t){if(d===clearTimeout)return clearTimeout(t);if((d===U||!d)&&clearTimeout)return d=clearTimeout,clearTimeout(t);try{return d(t)}catch(e){try{return d.call(null,t)}catch(n){return d.call(this,t)}}}var f=[],S=!1,y,v=-1;function nt(){!S||!y||(S=!1,y.length?f=y.concat(f):v=-1,f.length&&V())}function V(){if(!S){var t=M(nt);S=!0;for(var e=f.length;e;){for(y=f,f=[];++v<e;)y&&y[v].run();v=-1,e=f.length}y=null,S=!1,et(t)}}function rt(t){var e=new Array(arguments.length-1);if(arguments.length>1)for(var n=1;n<arguments.length;n++)e[n-1]=arguments[n];f.push(new O(t,e)),f.length===1&&!S&&M(V)}function O(t,e){this.fun=t,this.array=e}O.prototype.run=function(){this.fun.apply(null,this.array)};var it="browser",at="browser",ct=!0,st=[],ot="",ut={},lt={},ft={};function p(){}var ht=p,mt=p,gt=p,dt=p,wt=p,yt=p,pt=p;function bt(t){throw new Error("process.binding is not supported")}function St(){return"/"}function xt(t){throw new Error("process.chdir is not supported")}function Et(){return 0}var x=b.performance||{},vt=x.now||x.mozNow||x.msNow||x.oNow||x.webkitNow||function(){return new Date().getTime()};function Tt(t){var e=vt.call(x)*.001,n=Math.floor(e),r=Math.floor(e%1*1e9);return t&&(n=n-t[0],r=r-t[1],r<0&&(n--,r+=1e9)),[n,r]}var Rt=new Date;function Ct(){var t=new Date,e=t-Rt;return e/1e3}var Pt={nextTick:rt,title:it,browser:ct,env:{NODE_ENV:"production"},argv:st,version:ot,versions:ut,on:ht,addListener:mt,once:gt,off:dt,removeListener:wt,removeAllListeners:yt,emit:pt,binding:bt,cwd:St,chdir:xt,umask:Et,hrtime:Tt,platform:at,release:lt,config:ft,uptime:Ct};function Lt(t,e){if(t.match(/^[a-z]+:\/\//i))return t;if(t.match(/^\/\//))return window.location.protocol+t;if(t.match(/^[a-z]+:/i))return t;const n=document.implementation.createHTMLDocument(),r=n.createElement("base"),i=n.createElement("a");return n.head.appendChild(r),n.body.appendChild(i),e&&(r.href=e),i.href=t,i.href}const $t=(()=>{let t=0;const e=()=>`0000${(Math.random()*36**4<<0).toString(36)}`.slice(-4);return()=>(t+=1,`u${e()}${t}`)})();function h(t){const e=[];for(let n=0,r=t.length;n<r;n++)e.push(t[n]);return e}function T(t,e){const r=(t.ownerDocument.defaultView||window).getComputedStyle(t).getPropertyValue(e);return r?parseFloat(r.replace("px","")):0}function At(t){const e=T(t,"border-left-width"),n=T(t,"border-right-width");return t.clientWidth+e+n}function Dt(t){const e=T(t,"border-top-width"),n=T(t,"border-bottom-width");return t.clientHeight+e+n}function $(t,e={}){const n=e.width||At(t),r=e.height||Dt(t);return{width:n,height:r}}function It(){let t,e;try{e=Pt}catch(r){}const n=e&&e.env?e.env.devicePixelRatio:null;return n&&(t=parseInt(n,10),Number.isNaN(t)&&(t=1)),t||window.devicePixelRatio||1}const l=16384;function Ft(t){(t.width>l||t.height>l)&&(t.width>l&&t.height>l?t.width>t.height?(t.height*=l/t.width,t.width=l):(t.width*=l/t.height,t.height=l):t.width>l?(t.height*=l/t.width,t.width=l):(t.width*=l/t.height,t.height=l))}function kt(t,e={}){return t.toBlob?new Promise(n=>{t.toBlob(n,e.type?e.type:"image/png",e.quality?e.quality:1)}):new Promise(n=>{const r=window.atob(t.toDataURL(e.type?e.type:void 0,e.quality?e.quality:void 0).split(",")[1]),i=r.length,a=new Uint8Array(i);for(let c=0;c<i;c+=1)a[c]=r.charCodeAt(c);n(new Blob([a],{type:e.type?e.type:"image/png"}))})}function R(t){return new Promise((e,n)=>{const r=new Image;r.decode=()=>e(r),r.onload=()=>e(r),r.onerror=n,r.crossOrigin="anonymous",r.decoding="async",r.src=t})}async function Ut(t){return Promise.resolve().then(()=>new XMLSerializer().serializeToString(t)).then(encodeURIComponent).then(e=>`data:image/svg+xml;charset=utf-8,${e}`)}async function Mt(t,e,n){const r="http://www.w3.org/2000/svg",i=document.createElementNS(r,"svg"),a=document.createElementNS(r,"foreignObject");return i.setAttribute("width",`${e}`),i.setAttribute("height",`${n}`),i.setAttribute("viewBox",`0 0 ${e} ${n}`),a.setAttribute("width","100%"),a.setAttribute("height","100%"),a.setAttribute("x","0"),a.setAttribute("y","0"),a.setAttribute("externalResourcesRequired","true"),i.appendChild(a),a.appendChild(t),Ut(i)}const u=(t,e)=>{if(t instanceof e)return!0;const n=Object.getPrototypeOf(t);return n===null?!1:n.constructor.name===e.name||u(n,e)};function Vt(t){const e=t.getPropertyValue("content");return`${t.cssText} content: '${e.replace(/'|"/g,"")}';`}function Ot(t){return h(t).map(e=>{const n=t.getPropertyValue(e),r=t.getPropertyPriority(e);return`${e}: ${n}${r?" !important":""};`}).join(" ")}function Ht(t,e,n){const r=`.${t}:${e}`,i=n.cssText?Vt(n):Ot(n);return document.createTextNode(`${r}{${i}}`)}function H(t,e,n){const r=window.getComputedStyle(t,n),i=r.getPropertyValue("content");if(i===""||i==="none")return;const a=$t();try{e.className=`${e.className} ${a}`}catch(s){return}const c=document.createElement("style");c.appendChild(Ht(a,n,r)),e.appendChild(c)}function _t(t,e){H(t,e,":before"),H(t,e,":after")}const _="application/font-woff",q="image/jpeg",qt={woff:_,woff2:_,ttf:"application/font-truetype",eot:"application/vnd.ms-fontobject",png:"image/png",jpg:q,jpeg:q,gif:"image/gif",tiff:"image/tiff",svg:"image/svg+xml",webp:"image/webp"};function Wt(t){const e=/\.([^./]*?)$/g.exec(t);return e?e[1]:""}function A(t){const e=Wt(t).toLowerCase();return qt[e]||""}function Bt(t){return t.split(/,/)[1]}function D(t){return t.search(/^(data:)/)!==-1}function W(t,e){return`data:${e};base64,${t}`}async function B(t,e,n){const r=await fetch(t,e);if(r.status===404)throw new Error(`Resource "${r.url}" not found`);const i=await r.blob();return new Promise((a,c)=>{const s=new FileReader;s.onerror=c,s.onloadend=()=>{try{a(n({res:r,result:s.result}))}catch(o){c(o)}},s.readAsDataURL(i)})}const I={};function jt(t,e,n){let r=t.replace(/\?.*/,"");return n&&(r=t),/ttf|otf|eot|woff2?/i.test(r)&&(r=r.replace(/.*\//,"")),e?`[${e}]${r}`:r}async function F(t,e,n){const r=jt(t,e,n.includeQueryParams);if(I[r]!=null)return I[r];n.cacheBust&&(t+=(/\?/.test(t)?"&":"?")+new Date().getTime());let i;try{const a=await B(t,n.fetchRequestInit,({res:c,result:s})=>(e||(e=c.headers.get("Content-Type")||""),Bt(s)));i=W(a,e)}catch(a){i=n.imagePlaceholder||"";let c=`Failed to fetch resource: ${t}`;a&&(c=typeof a=="string"?a:a.message),c&&console.warn(c)}return I[r]=i,i}async function zt(t){const e=t.toDataURL();return e==="data:,"?t.cloneNode(!1):R(e)}async function Gt(t,e){if(t.currentSrc){const a=document.createElement("canvas"),c=a.getContext("2d");a.width=t.clientWidth,a.height=t.clientHeight,c==null||c.drawImage(t,0,0,a.width,a.height);const s=a.toDataURL();return R(s)}const n=t.poster,r=A(n),i=await F(n,r,e);return R(i)}async function Xt(t){var e;try{if((e=t==null?void 0:t.contentDocument)===null||e===void 0?void 0:e.body)return await C(t.contentDocument.body,{},!0)}catch(n){}return t.cloneNode(!1)}async function Qt(t,e){return u(t,HTMLCanvasElement)?zt(t):u(t,HTMLVideoElement)?Gt(t,e):u(t,HTMLIFrameElement)?Xt(t):t.cloneNode(!1)}const Jt=t=>t.tagName!=null&&t.tagName.toUpperCase()==="SLOT";async function Kt(t,e,n){var r,i;let a=[];return Jt(t)&&t.assignedNodes?a=h(t.assignedNodes()):u(t,HTMLIFrameElement)&&((r=t.contentDocument)===null||r===void 0?void 0:r.body)?a=h(t.contentDocument.body.childNodes):a=h(((i=t.shadowRoot)!==null&&i!==void 0?i:t).childNodes),a.length===0||u(t,HTMLVideoElement)||await a.reduce((c,s)=>c.then(()=>C(s,n)).then(o=>{o&&e.appendChild(o)}),Promise.resolve()),e}function Yt(t,e){const n=e.style;if(!n)return;const r=window.getComputedStyle(t);r.cssText?(n.cssText=r.cssText,n.transformOrigin=r.transformOrigin):h(r).forEach(i=>{let a=r.getPropertyValue(i);i==="font-size"&&a.endsWith("px")&&(a=`${Math.floor(parseFloat(a.substring(0,a.length-2)))-.1}px`),u(t,HTMLIFrameElement)&&i==="display"&&a==="inline"&&(a="block"),i==="d"&&e.getAttribute("d")&&(a=`path(${e.getAttribute("d")})`),n.setProperty(i,a,r.getPropertyPriority(i))})}function Zt(t,e){u(t,HTMLTextAreaElement)&&(e.innerHTML=t.value),u(t,HTMLInputElement)&&e.setAttribute("value",t.value)}function Nt(t,e){if(u(t,HTMLSelectElement)){const n=e,r=Array.from(n.children).find(i=>t.value===i.getAttribute("value"));r&&r.setAttribute("selected","")}}function te(t,e){return u(e,Element)&&(Yt(t,e),_t(t,e),Zt(t,e),Nt(t,e)),e}async function ee(t,e){const n=t.querySelectorAll?t.querySelectorAll("use"):[];if(n.length===0)return t;const r={};for(let a=0;a<n.length;a++){const s=n[a].getAttribute("xlink:href");if(s){const o=t.querySelector(s),w=document.querySelector(s);!o&&w&&!r[s]&&(r[s]=await C(w,e,!0))}}const i=Object.values(r);if(i.length){const a="http://www.w3.org/1999/xhtml",c=document.createElementNS(a,"svg");c.setAttribute("xmlns",a),c.style.position="absolute",c.style.width="0",c.style.height="0",c.style.overflow="hidden",c.style.display="none";const s=document.createElementNS(a,"defs");c.appendChild(s);for(let o=0;o<i.length;o++)s.appendChild(i[o]);t.appendChild(c)}return t}async function C(t,e,n){return!n&&e.filter&&!e.filter(t)?null:Promise.resolve(t).then(r=>Qt(r,e)).then(r=>Kt(t,r,e)).then(r=>te(t,r)).then(r=>ee(r,e))}const j=/url\((['"]?)([^'"]+?)\1\)/g,ne=/url\([^)]+\)\s*format\((["']?)([^"']+)\1\)/g,re=/src:\s*(?:url\([^)]+\)\s*format\([^)]+\)[,;]\s*)+/g;function ie(t){const e=t.replace(/([.*+?^${}()|\[\]\/\\])/g,"\\$1");return new RegExp(`(url\\(['"]?)(${e})(['"]?\\))`,"g")}function ae(t){const e=[];return t.replace(j,(n,r,i)=>(e.push(i),n)),e.filter(n=>!D(n))}async function ce(t,e,n,r,i){try{const a=n?Lt(e,n):e,c=A(e);let s;if(i){const o=await i(a);s=W(o,c)}else s=await F(a,c,r);return t.replace(ie(e),`$1${s}$3`)}catch(a){}return t}function se(t,{preferredFontFormat:e}){return e?t.replace(re,n=>{for(;;){const[r,,i]=ne.exec(n)||[];if(!i)return"";if(i===e)return`src: ${r};`}}):t}function z(t){return t.search(j)!==-1}async function G(t,e,n){if(!z(t))return t;const r=se(t,n);return ae(r).reduce((a,c)=>a.then(s=>ce(s,c,e,n)),Promise.resolve(r))}async function P(t,e,n){var r;const i=(r=e.style)===null||r===void 0?void 0:r.getPropertyValue(t);if(i){const a=await G(i,null,n);return e.style.setProperty(t,a,e.style.getPropertyPriority(t)),!0}return!1}async function oe(t,e){await P("background",t,e)||await P("background-image",t,e),await P("mask",t,e)||await P("mask-image",t,e)}async function ue(t,e){const n=u(t,HTMLImageElement);if(!(n&&!D(t.src))&&!(u(t,SVGImageElement)&&!D(t.href.baseVal)))return;const r=n?t.src:t.href.baseVal,i=await F(r,A(r),e);await new Promise((a,c)=>{t.onload=a,t.onerror=c;const s=t;s.decode&&(s.decode=a),s.loading==="lazy"&&(s.loading="eager"),n?(t.srcset="",t.src=i):t.href.baseVal=i})}async function le(t,e){const r=h(t.childNodes).map(i=>X(i,e));await Promise.all(r).then(()=>t)}async function X(t,e){u(t,Element)&&(await oe(t,e),await ue(t,e),await le(t,e))}function fe(t,e){const{style:n}=t;e.backgroundColor&&(n.backgroundColor=e.backgroundColor),e.width&&(n.width=`${e.width}px`),e.height&&(n.height=`${e.height}px`);const r=e.style;return r!=null&&Object.keys(r).forEach(i=>{n[i]=r[i]}),t}const Q={};async function J(t){let e=Q[t];if(e!=null)return e;const r=await(await fetch(t)).text();return e={url:t,cssText:r},Q[t]=e,e}async function K(t,e){let n=t.cssText;const r=/url\(["']?([^"')]+)["']?\)/g,a=(n.match(/url\([^)]+\)/g)||[]).map(async c=>{let s=c.replace(r,"$1");return s.startsWith("https://")||(s=new URL(s,t.url).href),B(s,e.fetchRequestInit,({result:o})=>(n=n.replace(c,`url(${o})`),[c,o]))});return Promise.all(a).then(()=>n)}function Y(t){if(t==null)return[];const e=[],n=/(\/\*[\s\S]*?\*\/)/gi;let r=t.replace(n,"");const i=new RegExp("((@.*?keyframes [\\s\\S]*?){([\\s\\S]*?}\\s*?)})","gi");for(;;){const o=i.exec(r);if(o===null)break;e.push(o[0])}r=r.replace(i,"");const a=/@import[\s\S]*?url\([^)]*\)[\s\S]*?;/gi,c="((\\s*?(?:\\/\\*[\\s\\S]*?\\*\\/)?\\s*?@media[\\s\\S]*?){([\\s\\S]*?)}\\s*?})|(([\\s\\S]*?){([\\s\\S]*?)})",s=new RegExp(c,"gi");for(;;){let o=a.exec(r);if(o===null){if(o=s.exec(r),o===null)break;a.lastIndex=s.lastIndex}else s.lastIndex=a.lastIndex;e.push(o[0])}return e}async function he(t,e){const n=[],r=[];return t.forEach(i=>{if("cssRules"in i)try{h(i.cssRules||[]).forEach((a,c)=>{if(a.type===CSSRule.IMPORT_RULE){let s=c+1;const o=a.href,w=J(o).then(m=>K(m,e)).then(m=>Y(m).forEach(L=>{try{i.insertRule(L,L.startsWith("@import")?s+=1:i.cssRules.length)}catch(tt){console.error("Error inserting rule from remote css",{rule:L,error:tt})}})).catch(m=>{console.error("Error loading remote css",m.toString())});r.push(w)}})}catch(a){const c=t.find(s=>s.href==null)||document.styleSheets[0];i.href!=null&&r.push(J(i.href).then(s=>K(s,e)).then(s=>Y(s).forEach(o=>{c.insertRule(o,i.cssRules.length)})).catch(s=>{console.error("Error loading remote stylesheet",s)})),console.error("Error inlining remote css file",a)}}),Promise.all(r).then(()=>(t.forEach(i=>{if("cssRules"in i)try{h(i.cssRules||[]).forEach(a=>{n.push(a)})}catch(a){console.error(`Error while reading CSS rules from ${i.href}`,a)}}),n))}function me(t){return t.filter(e=>e.type===CSSRule.FONT_FACE_RULE).filter(e=>z(e.style.getPropertyValue("src")))}async function ge(t,e){if(t.ownerDocument==null)throw new Error("Provided element is not within a Document");const n=h(t.ownerDocument.styleSheets),r=await he(n,e);return me(r)}async function Z(t,e){const n=await ge(t,e);return(await Promise.all(n.map(i=>{const a=i.parentStyleSheet?i.parentStyleSheet.href:null;return G(i.cssText,a,e)}))).join(`
`)}async function de(t,e){const n=e.fontEmbedCSS!=null?e.fontEmbedCSS:e.skipFonts?null:await Z(t,e);if(n){const r=document.createElement("style"),i=document.createTextNode(n);r.appendChild(i),t.firstChild?t.insertBefore(r,t.firstChild):t.appendChild(r)}}async function N(t,e={}){const{width:n,height:r}=$(t,e),i=await C(t,e,!0);return await de(i,e),await X(i,e),fe(i,e),await Mt(i,n,r)}async function E(t,e={}){const{width:n,height:r}=$(t,e),i=await N(t,e),a=await R(i),c=document.createElement("canvas"),s=c.getContext("2d"),o=e.pixelRatio||It(),w=e.canvasWidth||n,m=e.canvasHeight||r;return c.width=w*o,c.height=m*o,e.skipAutoScale||Ft(c),c.style.width=`${w}`,c.style.height=`${m}`,e.backgroundColor&&(s.fillStyle=e.backgroundColor,s.fillRect(0,0,c.width,c.height)),s.drawImage(a,0,0,c.width,c.height),c}async function we(t,e={}){const{width:n,height:r}=$(t,e);return(await E(t,e)).getContext("2d").getImageData(0,0,n,r).data}async function ye(t,e={}){return(await E(t,e)).toDataURL()}async function pe(t,e={}){return(await E(t,e)).toDataURL("image/jpeg",e.quality||1)}async function be(t,e={}){const n=await E(t,e);return await kt(n)}async function Se(t,e={}){return Z(t,e)}export{Se as getFontEmbedCSS,be as toBlob,E as toCanvas,pe as toJpeg,we as toPixelData,ye as toPng,N as toSvg};
