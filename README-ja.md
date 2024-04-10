# Unity-IngameConsole

実機で使用可能なコンソールシステムです。  
ログ表示のほか、テキストコマンドを打ち込むことで定義されたデバッグコマンドの実行ができます。

このリポジトリではコンソールのGUI機能は提供しません。  
GUIは本体側で作成するか、以下に置いたものを合わせて使用してください。

- R3+TextMeshPro:  

- UniRx+TextMeshPro:  
https://github.com/sh-kj/Unity-IngameConsoleView-TMP.git


## Requirement

UniRx or R3 を使用します。  
どちらかを選択してインストールしてください。

## インストール

Package Managerより以下のgit URLでインストールしてください。

for R3: https://github.com/sh-kj/Unity-IngameConsole.git?path=R3/IngameConsole  
for UniRx: https://github.com/sh-kj/Unity-IngameConsole.git?path=UniRx/IngameConsole  


# 使用方法

## 事前準備

インストールが完了すると`Create Command Injector`ウィンドウが出ます。  
Createボタンを押すと`Assets/`直下に`CommandInjector.cs`ファイルができます(移動可、asmdef下は非推奨)。  
このMonobehaviourからコンソールシステムが初期化されます。起動直後のシーン等に配置してください。

ウィンドウは`ConsoleCommands script is not found.`という表示となります。  
Createボタンを押すと`Assets/`直下に`ConsoleCommands.cs`ファイルができます(移動可、asmdef下は非推奨)。  
このクラスは編集可能で、デバッグコマンドを記述できます。

## デバッグコマンドの作成

`ConsoleCommands.cs` または別ファイルで`public static partial class ConsoleCommands`とし、  
ConsoleCommandsクラスに`public static void SomeCommand(string[] args)` といったメソッドを追加していくことでコンソールから呼び出せるデバッグコマンドが作成できます。

コンソールからの呼び出しコマンドはそのままメソッド名となり、スペースで区切られた文字列がargs引数に渡されます(`args[0]`は最初の文字列、すなわちコマンド名となる)

また、メソッドに`[CommandName("some-command")]`Attributeを付けることでメソッド名とは別にコマンド名を定義することが可能です。

## ランタイムの操作

`radiants.IngameConsole.Console`のメソッドやメンバを呼び出せばOKです。

`Log()`でコンソールにログを出せます。

GUI側は、`LogUpdateAsObservable` をsubscribeし、こちらの更新にしたがってGUIでログが流れるようにしてください。  
テキストコマンドが打ち込まれた時は、コマンドをスペースでSplitして`Execute()`を呼び出してください。
