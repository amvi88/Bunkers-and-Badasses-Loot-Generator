import * as htmlToImage from 'html-to-image';

const isSafari = /^((?!chrome|android).)*safari/i.test(navigator?.userAgent);
const isNotFirefox = navigator.userAgent.indexOf("Firefox") < 0;

const snapshotCreator = (id) => {

  return new Promise((resolve, reject) => {

    try {
      const scale = window.devicePixelRatio;
      const element = document.getElementById(id)
      htmlToImage
        .toBlob(element, {
          height: element.offsetHeight * scale,
          width: element.offsetWidth * scale,
          style: {
            transform: "scale(" + scale + ")",
            transformOrigin: "top left",
            width: element.offsetWidth + "px",
            height: element.offsetHeight + "px",
          },
          useCorsEverywhereProxy: true
        })
        .then((blob) => {
          resolve(blob);
        });

    } catch (e) {
      reject(e);
    }
  });
};

const copyImageToClipBoardSafari = (id) => 
{
    navigator.clipboard
      .write([
        new ClipboardItem({
          "image/png": new Promise(async (resolve, reject) => {
            try {
              const blob = await snapshotCreator(id);
              resolve(new Blob([blob], { type: "image/png" }));
            } catch (err) {
              reject(err);
            }
          }),
        }),
      ])
      .then(() => {
      })
      .catch((err) => {
        console.error("Error:", err)
      });
}

const copyImageToClipBoardOtherBrowsers = (id) => {
   var permission = isNotFirefox? "clipboard-write" : "clipboardWrite";
    navigator?.permissions?.query({ name: permission })
      .then(async (result) => {
        if (result.state === "granted") {
          const type = "image/png";
          const blob = await snapshotCreator(id);
          let data = [new ClipboardItem({ [type]: blob })];
          navigator.clipboard
            .write(data)
            .then(() => {
            })
            .catch((err) => {
              console.error("Error:", err)
            });
        }
    });
  }


export function downloadCard(id, filename)
{
  var node = document.getElementById(id);
  htmlToImage
  .toPng(node)
  .then(function (dataUrl) {
    var link = document.createElement('a');
        link.download = filename;
        link.href = dataUrl;
        link.click();
  })
  .catch(function (error) {
    console.error("oops, something went wrong!", error);
  });
}

export function copyCard(id){
  if (isSafari)
  {
    copyImageToClipBoardSafari(id)
  }
  else
  {
    copyImageToClipBoardOtherBrowsers(id)
  }  
}

