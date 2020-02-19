import React, { Component } from 'react';
import { Card, CardTitle, CardBody } from 'reactstrap';
import PropTypes from 'prop-types';


export class LeaderboardCard extends Component {
    static displayName = LeaderboardCard.name;
    render() {
        this.props.leaderboard.sort((a, b) => LeaderboardCard.sortByCategory(a, b, this.props.category))
        return (
            <Card className="leaderboard-card">
                <CardTitle>{this.props.title}</CardTitle>
                <CardBody>
                    {this.formatData()}
                </CardBody>
            </Card>
        )
    }

    static sortByCategory(a, b, category) {
        if (category === 'averageGoalsAgainstPerGame') {
            return a[category] - b[category];
        }
        else {
            return b[category] - a[category];
        }
    }

    formatData() {
        switch (this.props.type) {
            case 'integer':
                return <ol>
                    {this.props.leaderboard.slice(0, 5).map(data =>
                        <li key={data.player.id + this.props.category}><strong>{data.player.firstName} {data.player.lastName}:</strong> {data[this.props.category]}</li>)}
                </ol>
            case 'percent':
                return <ol>
                    {this.props.leaderboard.slice(0, 5).map(data =>
                        <li key={data.player.id}><strong>{data.player.firstName} {data.player.lastName}:</strong> {data[this.props.category].toFixed(2)}%</li>)}
                </ol>
            default:
                return <ol>
                    {this.props.leaderboard.slice(0, 5).map(data =>
                        <li key={data.player.id}><strong>{data.player.firstName} {data.player.lastName}:</strong> {data[this.props.category].toFixed(2)}</li>)}
                </ol>
        }
    }

}
LeaderboardCard.propTypes = {
    leaderboard: PropTypes.array.isRequired,
    category: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    type: PropTypes.string
};