# FMusic

#### 介绍
C# WPF MusicPlayer Student
本设计实例实现了音乐播放、音乐播放模式选择，窗体风格选择、音乐like,歌词显示,读取MP3flac文件封面等功能，


### 图片
![Image Description](image1.png)
#### 说明
这是一个学生学习c#，完成大作业的一个c#wpf程序。
由于本人还在学习，时间精力有限，程序还存在一点bug以后有时间再优化。
该项目是借鉴 原文链接：https://blog.csdn.net/iceberg7012/article/details/117695916 ，在此基础上进行了更改，优化。
在此十分感谢大佬。



#### 安装教程
使用NuGet 导入了
1.  NAudio
2.  Taglibshape


#### 文件说明
1. 程序有一个窗体 Music.xaml,及其cs文件
2. MusicFiles 是音乐文件
3. img是程序静态图片
4. lrc是歌词文件
5. path是保存文件Love的list

### 目前存在的问题
1. 没有一个良好的播放列表
2. 部分lrc文件的时间格式不同，无法读取歌词
3. 部分文件不含cover，程序崩溃
4. 没有添加到其他播放列表的功能
5. 切换歌曲，lrc没有清除
6。播放音乐Kurousa-P、初音未来 - 千本桜 (千本樱) (Game Version).flac 点下一首有问题


###  总结
本实例为近期关于WPF、C#语言学习的一次实战训练，项目进行阶段遇到了许多问题，C#基础语法掌握不够牢固，WPF控件运用的熟练度有待进一步提高，总之，未来还会继续加强相关方面的学习，以期更大的进步。
项目源码部分参照、借鉴了诸多大佬，由于记录未能保存，这里很难一一列举，十分抱歉，同时也对大佬的分享表示感谢，另，若有引用本文实例，请注明来源，感谢支持。
