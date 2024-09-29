# Unity Project - SpinePlayerApp

## 背景
Spineアニメーターからの依頼で、**1年分（合計12作品）のアニメーション動画作品**をUnityで**インタラクティブプレイヤーアプリ**として統合するという案件があり、このプロジェクトは該当案件で使われたフレームワークの一部です。
プロジェクトの目的は、複数のアニメーション作品を統合し、インタラクティブな要素を追加することで、ユーザーが自由に操作できるアプリケーションを提供することです。


## プロジェクト概要
このUnityプロジェクトは、Spineのアニメーションやサウンド管理を行うためのシンプルなシステムを実装しています。また、UIボタンの動作やステータス管理も含まれてます。

### 主なスクリプト
- **AnimationManager.cs**: Spineアニメーションの制御と連動した音声再生制御を行うクラス
- **ButtonManager.cs**: UIのボタン・再生状態を示すランプを制御するクラス
- **SoundManager.cs**: 汎用的な再生制御マネージャー

### Unityバージョン
- **2022.3.19f1**

### 使用している外部パッケージ
- **spine-unity-4.2-2024-08-08.unitypackage**: Spine公式が提供するUnity用プラグイン - spine-unity 4.2

## デモ・スクリーンショット
![image](https://github.com/user-attachments/assets/5f3e723c-fc30-4434-9b6f-57fec6aeb5e4)

---

## 使用例 - インタラクティブなSpineアニメーションプレイヤー

このプロジェクトの使用例として、**Spineで作成された2Dアニメーションを再生するインタラクティブなプレイヤーアプリ**を構築することができます。

### 1. アプリの概要
- アプリは、4つの主要なセクション（**A, B, C, D**）に分かれ、UIボタンで各セクションに対応するアニメーションへ遷移します。
- **Spineのレイヤー機能**を活用し、動きのアニメーションとセリフと同期した口の動きを別のレイヤーで管理し、同時に再生します。
- アニメーションに加え、各セクションごとの**ループ音声**も実装されており、動画形式では難しかった柔軟な音声管理が可能です。

### 2. 各セクションの仕様
- **A, B, Dセクション**では、動きのアニメーションがループします。
- **Cセクション**では、アニメーションが1回再生された後、自動的に**Dセクション**に遷移します。

### 3. 複数パターンの管理
- **A, Bセクション**には複数のアニメーションパターンが存在し、各動画によって異なります。これをリストで動的に管理し、柔軟に対応できるようにしています。

### 4. Mix部分の再現
- **Mix部分**では、Spine側で一つのアニメーションに全ての動きを統合し、**疑似的に動画の再生内容を再現**しています。

---

## デモシーンについて
- **デモシーン**では、便宜的にSpineのサンプルアセットを使用しています。これらのアセットは公式で提供されているもので、動作確認とデモ目的のために使用されています。
- 実際のプロジェクトでは、依頼主から提供される**非公開のSpineアセット**を使用しますが、これらは公開不可のためデモには含まれておりません。

---





