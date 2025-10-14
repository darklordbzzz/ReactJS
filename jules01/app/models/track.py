class Track:
    def __init__(self, data):
        self.album = data.get('album', {})
        self.artists = data.get('artists', [])
        self.available_markets = data.get('available_markets', [])
        self.disc_number = data.get('disc_number')
        self.duration_ms = data.get('duration_ms')
        self.explicit = data.get('explicit')
        self.external_urls = data.get('external_urls', {})
        self.href = data.get('href')
        self.id = data.get('id')
        self.is_playable = data.get('is_playable')
        self.name = data.get('name')
        self.popularity = data.get('popularity')
        self.preview_url = data.get('preview_url')
        self.track_number = data.get('track_number')
        self.type = data.get('type')
        self.uri = data.get('uri')
        self.is_local = data.get('is_local')

    def __repr__(self):
        return f"<Track {self.name}>"