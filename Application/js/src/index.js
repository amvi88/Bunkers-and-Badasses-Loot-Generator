import * as htmlToImage from 'html-to-image';
import { toPng, toJpeg, toBlob, toPixelData, toSvg } from 'html-to-image';


export function downloadCard(id)
{
  var node = document.getElementById(id);
    htmlToImage.toPng(node)
    .then(function (dataUrl) {
        var link = document.createElement('a');
        link.download = 'gun.png';
        link.href = dataUrl;
        link.click();
      }).catch(function (error) {
        console.error('oops, something went wrong!', error);
      });
}