
# AnnyConnectController

[English README](README.md)

## 概要
- １クリックで、Cisco AnyConnect Secure Mobility Client に自動で PIN を入力するツールです

- PIN は Data Protection API (DPAPI) で暗号化して保存しています

- PIN ではなく Password を使う場合（がある？）の場合の挙動は未確認です


## インストール方法
- AnyConnectController.exe と AnyConnectController.exe.Config を任意のディレクトリに置いてください


## アンインストール方法
- AnyConnectController.exe と AnyConnectController.exe.Config を削除してください
- レジストリは使用してないです


## 使用方法
- 起動して、PIN を入力して、Connect ボタンを押すだけです。PIN は保存されるので楽になります。


## 開発メモ
- Cisco AnyConnect Secure Mobility Client の PATH は環境毎に違うかもしれません、その場合は Constants class の FULL_PATH_ANY_CONNECT を修正して、ビルドし直してください

- CLI の vpncli.exe が用意されてて、コマンドラインからは自動で PIN を入力して接続・切断は可能です。が、Cisco AnyConnect Secure Mobility Client が起動していると、接続はできないので、Cisco AnyConnect Secure Mobility Client を叩くのを作ってみました。

    - in.txt に Username と PIN を書いて、
      ```
      Username
      PIN
      ```
      以下で接続
      ```
      vpncli.exe -s connect <server> < in.txt
      ```
      以下で切断
      ```
      vpncli.exe disconnect
      ```
 

## History
- Ver.1.0.0
  - 最初のリリース


## ライセンス
- MIT

## その他
- [寄付](https://www.buymeacoffee.com/saltpp)