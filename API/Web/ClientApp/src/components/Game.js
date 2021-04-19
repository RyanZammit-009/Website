import React, { Component } from 'react';
import Button from 'react-bootstrap/Button';
var $ = require('jquery');

export class Game extends Component {
  static displayName = Game.name;
  pointsPerQuestion = 5;

  getQuestion() {
    var questions = this.state.questions.filter(x => !x.isAnswered)
    if (questions.length > 0) {
      return questions[0];
    }
    return null;
  }

  base = () => {
    return (
      <div>
        {this.score()}
        <h2>The question is: {
          this.getQuestion().text
        }</h2>
        {this.getQuestion().answer.map(answer =>
          <Button onClick={() => this.handleClick(answer.isCorrectAnswer)}>{answer.text}</Button>
        )}
      </div>
    );
  }
  corr = () => { return (<div><h2>Correct</h2>{this.base()}</div>); }
  incorr = () => { return (<div><h2>Incorrect</h2>{this.base()}</div>); }
  endincorr = () => {
    return (
      <div>
        <h2>Incorrect</h2>
        {this.endbase()}
      </div>
    );
  }
  endcorr = () => {
    return (
      <div>
        <h2>Correct</h2>
        {this.endbase()}
      </div>
    );
  }
  endbase = () => {
    return (
      <div>
        {this.score()}
        <h2>There are no more questions</h2>
      </div>
    );
  }
  score = () => {
    return (
      <div>
        <h2>Score: {this.state.score}</h2>
      </div>
    );
  }

  constructor(props) {
    super(props);
    this.state = { questions: [], counter: 0, score: 0 };
  }

  componentDidMount() {
    var urlSearchParams = new URLSearchParams(document.location.search);
    var params = urlSearchParams.get('user_id');
    this.populateQuestionData(params);
  }

  render() {
    if (this.getQuestion()) {
      if (this.state.counter > 0) {
        return this.state.lastQuestion ? this.corr() : this.incorr()
      }
      else {
        return this.base();
      }
    } else {
      if (this.state.lastQuestion == null) {
        return this.endbase();
      }
      else if (this.state.lastQuestion) {
        return this.endcorr();
      }
      else if (!this.state.lastQuestion) {
        return this.endincorr();
      }
    }
  }

  handleClick = (correct) => {
    this.postAnswerChosen(correct);
    this.setState((state) => {
      var score = state.score + (correct ? this.pointsPerQuestion : 0);
      var questions = state.questions;
      questions.splice(0, 1);
      return { questions: questions, lastQuestion: correct, score: score }
    })
  };

  async populateQuestionData(id) {
    if (id) {
      const response = await fetch('question/api/' + id);
      const data = await response.json();
      this.setState({ questions: data, score: data.filter(x => x.isCorrect).length * this.pointsPerQuestion });
    }
  }

  async postAnswerChosen(correct) {
    await $.ajax({
      type: "POST",
      data :JSON.stringify({ user: this.state.questions[0].playerID, isCorrect: correct }),
      url: "question/api/post",
      contentType: "application/json"
  });
  }
}
