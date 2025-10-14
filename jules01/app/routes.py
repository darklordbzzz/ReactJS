from app import app
from flask import render_template, jsonify

@app.route('/')
@app.route('/index')
def index():
    return render_template('index.html')

@app.route('/api/data')
def api_data():
    return jsonify({'message': 'Hello from the API!'})

from app.spotify_client import SpotifyAPI

@app.route('/spotify')
def spotify():
    spotify_client = SpotifyAPI()
    # Using the track ID from the example
    track = spotify_client.get_track("11dFghVXANMlKmJXsNCbNl")
    return render_template('spotify.html', track=track)
