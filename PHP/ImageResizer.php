/**
 *        DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE 
 *                   Version 2, December 2004 
 *
 *     Copyright (C) 2004 Sam Hocevar <sam@hocevar.net> 
 *
 * Everyone is permitted to copy and distribute verbatim or modified 
 * copies of this license document, and changing it is allowed as long 
 * as the name is changed. 
 *
 *          DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE 
 *  TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION 
 *
 *  0. You just DO WHAT THE FUCK YOU WANT TO.
 */

/**
 * Compresses an Image and saves it to a destination.
 * @param String $source Image file to compress.
 * @param Double $quality The quality to compress.
 * @param String $destination If set, is the specified location where to save the compressed image.
 * @return The compressed image file. 
 * @example usage : compress("Image1.png", 0.7, "ImageCompressed.jpg");
 */
function compress($source, $quality = 0.7, $destination = $source) {
    $info = getimagesize($source);
    if ($info['mime'] == 'image/jpeg') {
        $image = imagecreatefromjpeg($source);
        imagejpeg($image, $destination, $quality);
    } elseif ($info['mime'] == 'image/gif') { 
        $image = imagecreatefromgif($source);
        imagegif($image, $destination, $quality);
    } elseif ($info['mime'] == 'image/png') { 
        $image = imagecreatefrompng($source);
        imagepng($image, $destination, $quality);
    } else {
        throw new BadFunctionCallException("Invalid Image format");
    }
	return $destination;
}
