/*
TODO:
    Feature:
        implement bioengineering lab, market, solar panels, factory, wonk! chemical plant, wonk! hq, gold mine, blood shrine, fortune teller, junkyard, fungus farm, spore tower, shortcut

    Bug:
        tiles rendering on top of draftqueueitems



tiles generate population, money, energy

population:
    ultimate goal is to increase population. typically costs money and/or energy to generate.
    static resource.

money:
    used for purchasing tiles and operating certain tiles. cumulative resource.

energy:
    used for operating high-cost tiles. cumulative resource.

the players' goal is to achieve 100 population before the other players. This is typically done
by acquiring exponentially more powerful tiles as the game progresses, eventually building a
resource production base that is capable of supporting 100 population. The player may take a
variety of actions to accomplish this.

actions:
    purchase tiles:
        purchasing mines or solar panels:
            at any time, players may purchase mines and solar panels. They produce 1 money and
            1 energy respectively, and are intended as a way to produce a base of resources
            to fund the operation/purchase of advanced tiles.
        purchasing other tiles:
            the player can choose to open a draft, allowing them to choose one of three tiles.
            This costs money, and the cost increases with each successive draft. These tiles
            are significantly more powerful than mines and solar panels, and potentially
            have additional costs of operation.






















*/
