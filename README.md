# Intelligent scissor with a Magnetic Lasso tool 

![Example 1](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/1.png  "Example 1")

![Example 2](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/2.png  "Example 2")


The Intelligent scissor is a tool which enables you to select an item or object from a picture, and reuse it as a separate item in another image, with the feature of the magnetic selection, this task became so much easier, it selects the object properly with high accuracy.

The PhotoShop App has a lasso tool, it helps you to crop objects from images:

[![PhotoShop Lasso tool](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/30.png)](https://www.youtube.com/watch?v=0rQEctxkjBMMy)

project simply take an image, and you put anchor points and use a live wire to walk through the image and to boundary the object you want to crop:

Here i’m walking through the image using my live wire(The blue line)

![Example 1](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/20.png  "Example 1")

After bounding the object, you can crop it via the Crop button, and here it’s:

![Example 2](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/21.png  "Example 2")

You can also crop simple object from the picture, for example:

![Example 3](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/22.png  "Example 3")

![Example 4](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/23.png  "Example 4")


You can activate also the auto Anchor, and it will automatically boundary the object by the live wire.

💫 **Version 1.0 is out now!**
    
📖 Documentation
================

## How to Run

Just run the launcher, and you will be able to use the app, it's based on C# Windows Forms

## Algorithm
The algorithm is based on the two papers:
1. Intelligent Scissors for Image Composition
2. Interactive Segmentation with Intelligent Scissors
Simply I'm using Dijkstra to get the shortest path between two points on the boundaries of the shape
based on the color intensity, and the magnetic lasso feature is based on the time of the freezed drawn line, the longer the line freezes, the better it is, so we use the one stayed longer

## Results

The results are very accurate, and the magnetic feature works smoothly and perfectly.

##### Notice the magnetic lasso feature, how close the line selects the object

![Example 1](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/3.png  "Example 1")

![Example 2](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/4.png  "Example 2")

![Example 3](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/5.png  "Example 3")

![Example 4](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/6.png  "Example 4")

![Example 5](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/7.png  "Example 5")

##### Notice how it crops the images perfectly too!

Origin 1                                                                                                         |  Cropped 1 
:---------------------------------------------------------------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------:
![Example 1](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/10.png  "Example 1")  |  ![Example 1](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/11.png  "Example 1")

Origin 2                                                                                                         |  Cropped 2 
:---------------------------------------------------------------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------:
![Example 2](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/12.jpg  "Example 2")  |  ![Example 2](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/13.png  "Example 2")

Origin 3                                                                                                         |  Cropped 3 
:---------------------------------------------------------------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------:
![Example 3](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/14.jpg  "Example 3")  |  ![Example 3](https://github.com/AbdelrahmanRadwan/Intelligent-Scissors/blob/master/results/15.png  "Example 3")


