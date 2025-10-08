# ToN Overlay

## これは何
[Terrors of Nowhere](https://vrchat.com/home/world/wrld_a61cdabe-1218-4287-9ffc-2a4d1414e5bd)向けのオーバーレイVRアプリケーションです。右腕に現在のテラーを表示します。<br>
使用には[ToNSaveManager](https://github.com/ChrisFeline/ToNSaveManager)が必要です。

## トラブルシューティング
### WebSocketが繋がらない
WebSocket API Serverを有効化してください。
![1732946882-2j5z6seSZTwX13lDhIqy4dKE](https://github.com/user-attachments/assets/93ac0d32-6e09-4d0a-a2f4-7cd97ccfc907)

### 表示位置が変
現時点ではPICO4でチューニングされているためです。このリポジトリをクローンして、UnityEditor上で`Overlay Preset`を調整してください。<br>
Presetのプルリクエスト大歓迎です。

## ライセンス
MIT Licenseです。[ThirdPartyNotices.txt](ThirdPartyNotices.txt)も併せてご確認ください。

## 謝辞
このアプリケーションの作成にあたり、kurohuku様の「Unity でつくる SteamVR オーバーレイアプリケーション」を参考にさせて頂きました。ありがとうございます。<br>
https://zenn.dev/kurohuku/books/a082c5728cc1f6
