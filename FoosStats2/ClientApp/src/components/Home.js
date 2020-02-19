import React, { Component } from 'react';
import { LeaderboardCard } from "../PageElements/LeaderboardCard";
import { CardDeck } from 'reactstrap';
import {RedVsBluePieChart} from '../PageElements/RedVsBluePieChart';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      winPercentage: [],
      averageGoalsPerGame: [],
      averageGoalsAgainstPerGame: [],
      offenceWinPct: [],
      defenseWinPct: [],
      redWinPct: [],
      blueWinPct: [],
      averageOffenseElo: [],
      averageDefenseElo: [],
      leaderboard: [],
      winPerSide: [],
      loading: true,
    };
  }


  componentDidMount() {
    this.populateAllData();
  }

  async populateAllData() {
    let response1 = await fetch('https://localhost:44308/api/leaderboard')
    let leaderboardData = await response1.json();
    let response2 = await fetch('https://localhost:44308/api/league/winperside')
    let graphData = await response2.json();
    this.setState({ leaderboard: leaderboardData, winPerSide: [{ name: "Blue", value: graphData[0] }, { name: "Red", value: graphData[1] }], loading: false });

  };

  render() {
    if (this.state.loading) {
      var contents = <p>Loading...</p>
    }
    else {
      contents = <div>
        
        <RedVsBluePieChart blueWins={this.state.winPerSide[0].value} redWins={this.state.winPerSide[1].value}/>
        <hr></hr>
        <h2>Leaderboards</h2>
        <CardDeck>
          <LeaderboardCard category='winPercentage' leaderboard={this.state.leaderboard} title='Win Percentage' type='percent' />
          <LeaderboardCard category='averageGoalsPerGame' leaderboard={this.state.leaderboard} title='Average Goals Per Game' />
          <LeaderboardCard category='averageGoalsAgainstPerGame' leaderboard={this.state.leaderboard} title='Average Goals Against Per Game' />
        </CardDeck>
        <CardDeck>
          <LeaderboardCard category='offenceWinPct' leaderboard={this.state.leaderboard} title='Offense Win Percentage' type='percent' />
          <LeaderboardCard category='defenseWinPct' leaderboard={this.state.leaderboard} title='Defense Win Percentage' type='percent' />
          <LeaderboardCard category='redWinPct' leaderboard={this.state.leaderboard} title='Red Win Percentage' type='percent' />
          <LeaderboardCard category='blueWinPct' leaderboard={this.state.leaderboard} title='Blue Win Percentage' type='percent' />
        </CardDeck>
        <CardDeck>
          <LeaderboardCard category='averageOffenseElo' leaderboard={this.state.leaderboard} title='Average Offense Elo' type='integer' />
          <LeaderboardCard category='averageDefenseElo' leaderboard={this.state.leaderboard} title='Average Defense Elo' type='integer' />
        </CardDeck>
        <hr></hr>
      </div>
    }
    return (
      <div>
        <h1>Welcome To FoosStats</h1>
        {contents}
      </div>

    );
  }

}
