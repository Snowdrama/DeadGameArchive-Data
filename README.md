# DeadGameArchive-Data
The Data used for the Dead Game Archive

This Repo is simply for storage for the JSON data for [The Dead Game Archive](https://deadgamearchive.com/) a website made by [John 'Snowdrama' Close](https://github.com/Snowdrama/)

The data is broken into a few parts as I'm expanding the site. 

# How it's organized:

The main data is in the `./Games` folder these are individual games listed as single json files, these get compiled later into what's actually read in by the site. Inside is `./Games/Images/` which is where each folder has a list of images for the game in question.

I'm also working on a `./Preserved` section for games that have positive stories of preservation like games that where the devs have done something to help preserve the game for the future like the game's source being released.

Finally I have `Blurbs.json` which is just the blurbs that show up on the home page lol.

I also plan to have stuff like `./Abandonware` and `./MMOs` at some point but those aren't ready. 

# The Shenanigans

Since all these things are individual JSON files, I made a small C# script to combine everything into bundled "page" files.

This lets games get added easily by just adding a new file, and then the script runs to generate the bundles for the site. 

The Page JSON files allow the site to be paginated easily(and some day I plan to lazy load them when you scroll to the bottom but for now pages)

I'm also working on search which will have a different condensed JSON file with just the names of all the games and the individual JSON files linked... Something like that... 
